using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CoreBluetooth;
using Foundation;
using PalmSens.Core.iOS.Comm.Devices;
using PalmSens.Devices;

namespace PalmSens.Core.iOS.Comm
{
    public class DiscoverDevices : IDisposable
    {
        private readonly CBUUID _vspServiceUUID = CBUUID.FromString("569a1101-b87f-490c-92cb-11ba5ea5167c");

        private static CBCentralManager _centralManager;
        private static BLECBCentralManagerDelegate _centralManagerDelegate;

        public event Action<Device> DeviceDiscovered { 
            add => CentralManagerDelegate.PeripheralDiscovered += value;
            remove => CentralManagerDelegate.PeripheralDiscovered -= value;
        }

        private BLECBCentralManagerDelegate CentralManagerDelegate => _centralManagerDelegate ?? (_centralManagerDelegate = new BLECBCentralManagerDelegate(_vspServiceUUID));

        public async Task<List<Device>> Discover(CancellationToken cancellationToken, int timeout = 10000)
        {
            List<Device> devices = new List<Device>();
            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            CentralManagerDelegate.TimeOut = timeout;

            Action<List<Device>> scanCompletedAction = (List<Device> bleDevices) =>
            {
                devices = bleDevices;
                taskCompletionSource.SetResult(true);
            };

            CentralManagerDelegate.ScanCompleted += scanCompletedAction;

            if (_centralManager == null)
            {
                _centralManager = new CBCentralManager(CentralManagerDelegate, CoreFoundation.DispatchQueue.CurrentQueue);
            }
            else
            {
                CentralManagerDelegate.Scan(_centralManager);
            }

            using (cancellationToken.Register(() =>
            {
                CentralManagerDelegate.CancelScan(_centralManager);
                taskCompletionSource.TrySetCanceled(cancellationToken);
            }))
            {
                try
                {
                    await taskCompletionSource.Task;
                }
                catch (TaskCanceledException ex)
                {
                    if (!ex.CancellationToken.IsCancellationRequested) throw;
                }
            }

            CentralManagerDelegate.ScanCompleted -= scanCompletedAction;
            return devices;
        }

        public void Dispose()
        {
            
        }
    }

    /// <summary>
    /// There are two ways you can find devices:
    /// 1. Wait for all devices to be found.
    /// 2. Register to the PeripheralDiscovered event which fires every time a device is found.
    /// </summary>
    internal class BLECBCentralManagerDelegate : CBCentralManagerDelegate
    {
        private readonly CBUUID _serviceUUID;
        private List<Device> _devices;
        System.Timers.Timer _timer;
        public int TimeOut { get; set; } = 1000;
        public event Action ScanStarted;
        public event Action ScanCancelled;
        public event Action<List<Device>> ScanCompleted;
        public event Action<Device> PeripheralDiscovered;
        public event Action PeripheralConnected;
        public event Action PeripheralDisconnected;

        public BLECBCentralManagerDelegate(CBUUID serviceUUID) : base()
        {
            _serviceUUID = serviceUUID;
        }

        public override void UpdatedState(CBCentralManager manager)
        {
            if (manager.State == CBCentralManagerState.PoweredOn)
            {
                Scan(manager);
            }
        }

        public void Scan(CBCentralManager manager)
        {
            ScanStarted?.Invoke();
            _devices = new List<Device>();
            manager.ScanForPeripherals(_serviceUUID);
            _timer = new System.Timers.Timer(TimeOut);
            _timer.Elapsed += (sender, e) =>
            {
                _timer.Stop();
                _timer.Dispose();
                manager.StopScan();
                ScanCompleted?.Invoke(_devices);
            };

            _timer.Start();
        }

        public void CancelScan(CBCentralManager manager)
        {
            _timer?.Stop();
            _timer?.Dispose();
            manager?.StopScan();
            ScanCancelled?.Invoke();
        }

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral,
            NSDictionary advertisementData, NSNumber RSSI)
        {
            Device device = new BLEDevice(peripheral, central);
            _devices.Add(device);
            PeripheralDiscovered?.Invoke(device);
        }

        public override void ConnectedPeripheral(CBCentralManager central, CBPeripheral peripheral)
        {
            PeripheralConnected?.Invoke();
        }

        public override void DisconnectedPeripheral(CBCentralManager central, CBPeripheral peripheral, NSError error)
        {
            PeripheralDisconnected?.Invoke();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            _timer?.Stop();
            _timer?.Dispose();

            ScanStarted = null;
            ScanCompleted = null;
            ScanCancelled = null;
            PeripheralDiscovered = null;
            PeripheralConnected = null;
            PeripheralDisconnected = null;
        }
    }
}