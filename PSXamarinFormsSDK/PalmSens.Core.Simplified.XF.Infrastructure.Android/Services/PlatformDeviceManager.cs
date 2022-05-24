using Android.Content;
using Android.OS;
using PalmSens.Comm;
using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using Plugin.CurrentActivity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PalmSens.Core.Simplified.XF.Infrastructure.Android.Services
{
    public class PlatformDeviceManager : IInstrumentPlatform
    {
        #region fields

        private DeviceHandler _deviceHandler;
        private Handler _mainHandler;

        public event EventHandler<PlatformDevice> DeviceDiscovered;

        private Context Context => CrossCurrentActivity.Current.AppContext;

        #endregion fields

        public PlatformDeviceManager()
        {
            PalmSens.PSAndroid.Utils.CoreDependencies.Init(Context);
            InitAsyncFunctionality(System.Environment.ProcessorCount);
            _mainHandler = new Handler(Looper.MainLooper);
            _deviceHandler = new DeviceHandler();
            _deviceHandler.DeviceDiscoverered += _deviceHandler_DeviceDiscoverered;
            _deviceHandler.Context = Context;
        }

        #region methods

        public Task<CommManager> Connect(Devices.Device device)
        {
            return _deviceHandler.ConnectAsync(device);
        }

        public Task Disconnect(CommManager comm)
        {
            return _deviceHandler.DisconnectAsync(comm);
        }

        public void Dispose()
        {
            DeviceDiscovered = null;
        }

        public async Task<List<PlatformDevice>> GetConnectedDevices(CancellationToken? cancellationToken = null)
        {
            var scannedDevices = await _deviceHandler.ScanDevicesAsync(cancellationToken, 5000);

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

        public bool InvokeIfRequired(Delegate method, params object[] args)
        {
            if (Looper.MyLooper() != Looper.MainLooper)//Check if event needs to be cast to the UI thread
            {
                _mainHandler.Post(() => method.DynamicInvoke(args)); //Recast event to UI thread
                return true;
            }
            return false;
        }

        private void _deviceHandler_DeviceDiscoverered(object sender, Devices.Device e)
        {
            if (InvokeIfRequired(new EventHandler<PalmSens.Devices.Device>(_deviceHandler_DeviceDiscoverered), e))
            {
                return;
            }

            DeviceDiscovered?.Invoke(this, new PlatformDevice() { Name = e.ToString(), Device = e });
        }

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

        #endregion methods
    }
}