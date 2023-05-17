using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PalmSens.Comm;
using PalmSens.Core.iOS;
using PalmSens.Core.iOS.Comm.Devices;
using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using Xamarin.Essentials;

namespace PalmSens.Core.Simplified.XF.Infrastructure.iOS.Services
{
    public class PlatformDeviceManager: IInstrumentPlatfrom
    {
        #region fields

        private DeviceHandler _deviceHandler;
        public event EventHandler<PlatformDevice> DeviceDiscovered;
        public event EventHandler<PlatformDevice> DeviceRemoved;

        #endregion

        public PlatformDeviceManager()
        {
            CoreDependencies.Init();
            InitAsyncFunctionality(System.Environment.ProcessorCount);
            _deviceHandler = new DeviceHandler();
            _deviceHandler.DeviceDiscovered += DeviceHandlerDeviceDiscovered;
        }

        #region methods
        /// <summary>
        /// Required initialization for using the async functionalities of the PalmSens SDK.
        /// The amount of simultaneous operations will be limited to prevent performance issues.
        /// When possible it will leave one core free for the UI.
        /// </summary>
        /// <param name="nCores">The number of CPU cores.</param>
        private void InitAsyncFunctionality(int nCores)
        {
            SynchronizationContextRemover.Init(nCores > 1 ? nCores - 1 : 1);
        }

        public async Task<List<PlatformDevice>> GetConnectedDevices(CancellationToken? cancellationToken = null)
        {
            var scannedDevices = await _deviceHandler.ScanDevicesAsync(cancellationToken);

            var platformDevices = new List<PlatformDevice>();
            foreach (var device in scannedDevices)
            {
                var platformDevice = new PlatformDevice();
                platformDevice.Name = device.ToString();
                platformDevice.Device = device;
                platformDevices.Add(platformDevice);
            }
            
            return platformDevices;
        }

        private void DeviceHandlerDeviceDiscovered(object sender, Devices.Device e)
        {
            if (InvokeIfRequired(new EventHandler<PalmSens.Devices.Device>(DeviceHandlerDeviceDiscovered), sender, e))
            {
                return;
            }

            DeviceDiscovered?.Invoke(this, new PlatformDevice() { Name = e.ToString(), Device = e });
        }

        public Task<CommManager> Connect(Devices.Device device)
        {
            return _deviceHandler.ConnectAsync(device);
        }

        public bool InvokeIfRequired(Delegate method, params object[] args)
        {
            if (!MainThread.IsMainThread)//Check if event needs to be cast to the UI thread
            {
                MainThread.BeginInvokeOnMainThread(() => method.DynamicInvoke(args)); //Recast event to UI thread                
                return true;
            }
            return false;
        }

        public Task Disconnect(CommManager comm)
        {
            return _deviceHandler.DisconnectAsync(comm);
        }

        public void Dispose()
        {
            DeviceDiscovered = null;
        }

        #endregion
    }
}