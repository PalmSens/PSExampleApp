using System;
using System.Threading;
using System.Threading.Tasks;
using CoreBluetooth;
using Foundation;
using PalmSens.Devices;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Text;

namespace PalmSens.Core.iOS.Comm.Devices
{
    public class BLEDevice : Device
    {
        #region fields
        private readonly CBPeripheral _peripheral;
        private readonly CBCentralManager _cbManager;
        private readonly BLECBCentralManagerDelegate _bleCBCentralManagerDelegate;
        private readonly string _name;

        private readonly CBUUID _vspServiceUUID = CBUUID.FromString("569a1101-b87f-490c-92cb-11ba5ea5167c");
        private PeripheralDelegate _peripheralDelegate = null;
        private TaskCompletionSource<bool> _openConnectionTCS;
        #endregion

        #region Properties
        public string ID { get; }
        #endregion

        public BLEDevice(CBPeripheral peripheral, CBCentralManager cbManager)
        {
            _peripheral = peripheral;
            ID = _peripheral.Identifier.AsString();
            _name = _peripheral.Name;
            
            _cbManager = cbManager;

            _bleCBCentralManagerDelegate = _cbManager.Delegate as BLECBCentralManagerDelegate;
        }

        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(_name) ? _name : "BLE Device";
        }

        #region methods
        public override void Open()
        {
            throw new NotImplementedException();
        }

        protected override async Task OpenAsyncInternal()
        {
            await Connect();
            _peripheral.Delegate = _peripheralDelegate = new PeripheralDelegate(); //new PeripheralDelegate(TaskScheduler.Current);
            await DiscoverCharacteristics();
        }

        private async Task Connect()
        {
            var connectResult = await AwaitEvent(() => { _cbManager.ConnectPeripheral(_peripheral); return true; },
                false,
                handler => _bleCBCentralManagerDelegate.PeripheralConnected += handler,
                handler => _bleCBCentralManagerDelegate.PeripheralConnected -= handler,
                5000);
            if (!connectResult.Completed) throw new Exception("Failed to connect to peripheral service");
        }

        private async Task DiscoverCharacteristics()
        {
            var connectResult = await AwaitEvent(() => { _peripheral.DiscoverServices(new CBUUID[] { _vspServiceUUID }); return true; },
                false,
                handler => _peripheralDelegate.CharacteristicsDiscovered += handler,
                handler => _peripheralDelegate.CharacteristicsDiscovered -= handler,
                5000);
            if (!connectResult.Completed) throw new Exception("Failed to discover read/write characteristics");
        }

        private Task<(TResult Result, bool Completed)> AwaitEvent<TResult>(Func<TResult> execute, TResult defaultResult, Action<Action> subscribeSuccessHandler, Action<Action> unsubscribeSuccessHandler, int timeOutInMilliSeconds)
        {
            return AwaitEvent(execute, defaultResult, subscribeSuccessHandler, unsubscribeSuccessHandler, handler => { }, handler => { }, timeOutInMilliSeconds);
        }

        private async Task<(TResult Result, bool Completed)> AwaitEvent<TResult>(Func<TResult> execute, TResult defaultResult, Action<Action> subscribeSuccessHandler, Action<Action> unsubscribeSuccessHandler, Action<Action> subscribeFailHandler, Action<Action> unsubscribeFailHandler, int timeOutInMilliSeconds)
        {
            TResult result = defaultResult;
            var tcs = new TaskCompletionSource<bool>();
            Action successHandler = () => tcs.SetResult(true);
            Action failHandler = () => tcs.SetResult(false);
            Timer timer = null;

            if (timeOutInMilliSeconds != Timeout.Infinite)
            {
                timer = new Timer(state =>
                {
                    var tcsFromState = (TaskCompletionSource<bool>)state;
                    tcsFromState.SetResult(false);
                }, tcs, timeOutInMilliSeconds, Timeout.Infinite);
            }

            try
            {
                subscribeSuccessHandler(successHandler);
                subscribeFailHandler(failHandler);
                result = execute();
                bool success = await tcs.Task;
                if (timer != null) await timer.DisposeAsync();
                return (result, success);
            }
            finally
            {
                unsubscribeFailHandler(failHandler);
                unsubscribeSuccessHandler(successHandler);
            }
        }


        public override void Open(int baudrate)
        {
            throw new NotImplementedException();
        }

        protected override async Task OpenAsyncInternal(int baudrate)
        {
            await base.OpenAsync();
        }

        public override string Read()
        {
            throw new NotImplementedException();
        }

        protected override async Task<string> ReadAsyncInternal()
        {
            DateTime readTimeOut = DateTime.Now + TimeSpan.FromMilliseconds(20);

            string read = "";
            while (string.IsNullOrEmpty(read) && DateTime.Now < readTimeOut)
            {
                read = await _peripheralDelegate.Read();
                if (string.IsNullOrEmpty(read))
                    await Task.Delay(5);
            }

#if DEBUGCOMM
                if (!string.IsNullOrEmpty(read)) Debug.WriteLine($"{DateTime.Now.Millisecond} READ: {read}");
#endif

            return read;
        }

        public override void Write(string s)
        {
            throw new NotImplementedException();
        }

        protected override async Task WriteAsyncInternal(string s)
        {
            int offset = 0;
            while (s.Length != offset)
            {
                int length = s.Length - offset > 20 ? 20 : s.Length - offset;
                await _peripheralDelegate.Write(s.Substring(offset, length), _peripheral, 2000);
                offset += length;
            }

#if DEBUGCOMM
                if (!string.IsNullOrEmpty(s)) Debug.WriteLine($"{DateTime.Now.Millisecond} WRITE: {s}");
#endif
        }

        public override void Close()
        {
            if (_peripheral.IsConnected) _cbManager.CancelPeripheralConnection(_peripheral);

            _peripheralDelegate.Dispose();
            _peripheralDelegate = null;
        }

        protected override async Task CloseAsyncInternal()
        {
            if (_peripheral.IsConnected)
            {
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

                void OnPeripheralDisconnected() => tcs.SetResult(true);

                _bleCBCentralManagerDelegate.PeripheralDisconnected += OnPeripheralDisconnected;
                _cbManager.CancelPeripheralConnection(_peripheral);
                await tcs.Task;
                _bleCBCentralManagerDelegate.PeripheralDisconnected -= OnPeripheralDisconnected;
            }

            _peripheralDelegate.Dispose();
            _peripheralDelegate = null;
        }

        #endregion

        #region properties
        public override int Baudrate => 230400;

        public override bool SupportsDownloading => false;

        public override bool IsWireless => true;
        #endregion

        private class PeripheralDelegate : CBPeripheralDelegate
        {
            //private readonly TaskScheduler _taskScheduler;
            private ConcurrentQueue<string> _queue = new ConcurrentQueue<string>();
            //private char[] _readBuffer = new char[4096];
            private readonly CBUUID _vspServiceUUID = CBUUID.FromString("569a1101-b87f-490c-92cb-11ba5ea5167c");
            private readonly CBUUID _incomingVSPCharacteristicUUID = CBUUID.FromString("569a2000-b87f-490c-92cb-11ba5ea5167c");
            private readonly CBUUID _outgoingVSPCharacteristicUUID = CBUUID.FromString("569a2001-b87f-490c-92cb-11ba5ea5167c");
            //private int _readAvailable = 0;
            public event Action CharacteristicsDiscovered;
            public event Action ValueWritten;
            //SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
            private TaskCompletionSource<bool> _tcs;
            private CBCharacteristic _outgoing = null;

            public PeripheralDelegate() // (TaskScheduler taskScheduler)
            {
                //_taskScheduler = taskScheduler;
            }

            public override void UpdatedCharacterteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, NSError error)
            {
                if (characteristic.UUID != _incomingVSPCharacteristicUUID) return;
                //char[] read = characteristic.Value.ToString(NSStringEncoding.ASCIIStringEncoding).ToString().ToCharArray();
                string read = characteristic.Value.ToString(NSStringEncoding.ASCIIStringEncoding);
                _queue.Enqueue(read);

                //var task = new TaskFactory(_taskScheduler).StartNew(async() => 
                //{
                //    Debug.WriteLine($"{DateTime.Now.Millisecond} BEFORE SEMAPHORE");
                //    await _semaphore.WaitAsync();
                //    Debug.WriteLine($"{DateTime.Now.Millisecond} AFTER SEMAPHORE");

                //    try
                //    {
                //        for (int i = 0; i < read.Length; i++)
                //        {
                //            _readBuffer[i + _readAvailable] = read[i];
                //        }

                //        _readAvailable += read.Length;
                //        Debug.WriteLine($"{DateTime.Now.Millisecond} READ BUFFER: {new string(_readBuffer).Substring(0, _readAvailable)}, READ AVAILABLE: {_readAvailable}");
                //    }
                //    finally { _semaphore.Release(); }
                //});

                //Debug.WriteLine(task.Status);
            }

            public override void WroteCharacteristicValue(CBPeripheral peripheral, CBCharacteristic characteristic, NSError error)
            {
                _tcs?.SetResult(error is null ? true : false); //TODO: Set to false on error
            }

            public async Task<string> Read()
            {
                //Debug.WriteLine("Awaiting reading");
                //await _semaphore.WaitAsync();
                //try
                //{
                //    Debug.WriteLine("Reading..");
                //    if (_readAvailable == 0) return "";
                //    string read = new string(_readBuffer, 0, _readAvailable);
                //    _readAvailable = 0;
                //    return read;
                //} catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //    throw;
                //}
                //finally { _semaphore.Release(); }

                StringBuilder stringBuilder = new StringBuilder();
                int queueCount = _queue.Count;
                string read;
                for (int i = 0; i < queueCount; i++)
                {
                    if (_queue.TryDequeue(out read))
                    {
                        stringBuilder.Append(read);
                    }
                }

                read = stringBuilder.ToString();
#if DEBUGCOMM
                Debug.WriteLine($"{DateTime.Now.Millisecond} READ: {read}");
#endif
                return read;
            }

            public async Task Write(string write, CBPeripheral peripheral, int timeout)
            {
                try
                {
                    //await _semaphore.WaitAsync();

                    _tcs = new TaskCompletionSource<bool>();
                    NSData data = NSData.FromString(write, NSStringEncoding.ASCIIStringEncoding);
                    peripheral.WriteValue(data, _outgoing, CBCharacteristicWriteType.WithResponse);

                    Timer timer = new Timer(state =>
                    {
                        var tcs = (TaskCompletionSource<bool>)state;
                        tcs.SetException(new Exception("Write failed"));
                    }, _tcs, timeout, Timeout.Infinite);

                    bool status = await _tcs.Task;
                    await timer.DisposeAsync();
                    if (!status) throw new Exception("Write failed");
                }
                finally
                {
#if DEBUGCOMM
                    Debug.WriteLine($"{DateTime.Now.Millisecond} WRITE: {write}");
#endif
                    //_semaphore.Release();
                }
                await Task.Delay(2);
            }

            public override void DiscoveredService(CBPeripheral peripheral, NSError error)
            {
                foreach (CBService service in peripheral.Services)
                {
                    if (service.UUID == _vspServiceUUID)
                    {
                        peripheral.DiscoverCharacteristics(new CBUUID[] { _incomingVSPCharacteristicUUID, _outgoingVSPCharacteristicUUID }, service);
                    }
                }
            }

            public override void DiscoveredCharacteristic(CBPeripheral peripheral, CBService service, NSError error)
            {
                CBCharacteristic incoming = null;

                foreach (CBCharacteristic characteristic in service.Characteristics)
                {
                    if (characteristic.UUID == _incomingVSPCharacteristicUUID)
                        incoming = characteristic;
                    if (characteristic.UUID == _outgoingVSPCharacteristicUUID)
                        _outgoing = characteristic;
                }

                if (incoming != null && _outgoing != null)
                {
                    peripheral.SetNotifyValue(true, incoming);
                    CharacteristicsDiscovered?.Invoke();
                }
            }
        }
    }
}