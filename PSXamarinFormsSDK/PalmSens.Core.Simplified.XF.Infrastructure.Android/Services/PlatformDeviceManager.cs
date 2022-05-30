using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using PalmSens.Comm;
using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.PSAndroid.Comm;
using Plugin.CurrentActivity;

namespace PalmSens.Core.Simplified.XF.Infrastructure.Android.Services
{
    public class PlatformDeviceManager: IInstrumentPlatfrom
    {
        #region fields

        private Context Context => CrossCurrentActivity.Current.AppContext;
        private DeviceHandler _deviceHandler;
        private Handler _mainHandler;
        public event EventHandler<PlatformDevice> DeviceDiscovered;
        public event EventHandler<PlatformDevice> DeviceRemoved;

        #endregion

        public PlatformDeviceManager()
        {
            PalmSens.PSAndroid.Utils.CoreDependencies.Init(Context);
            InitAsyncFunctionality(System.Environment.ProcessorCount);
            _mainHandler = new Handler(Looper.MainLooper);
            _deviceHandler = new DeviceHandler();
            _deviceHandler.DeviceDiscovered += DeviceHandlerDeviceDiscovered;
            _deviceHandler.DeviceRemoved += _deviceHandler_DeviceRemoved;
            _deviceHandler.Context = Context;
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
                if (device is UsbCdcDevice usbCdcDevice)
                {
                    platformDevice.DeviceID = usbCdcDevice.UsbDevice.DeviceId;
                }
                if (device is FTDIDevice ftdiDevice)
                {
                    platformDevice.DeviceID = ftdiDevice.UsbDevice.DeviceId;
                }
                platformDevices.Add(platformDevice);
            }
            
            return platformDevices;
        }

        private void DeviceHandlerDeviceDiscovered(object sender, Devices.Device e)
        {
            if (InvokeIfRequired(new EventHandler<PalmSens.Devices.Device>(DeviceHandlerDeviceDiscovered), e))
            {
                return;
            }

            DeviceDiscovered?.Invoke(this, new PlatformDevice() { Name = e.ToString(), Device = e, 
                DeviceID = 
                    e is FTDIDevice ftdiDevice 
                        ? (int?)ftdiDevice.UsbDevice.DeviceId 
                        : e is UsbCdcDevice usbCdcDevice 
                            ? (int?)usbCdcDevice.UsbDevice.DeviceId 
                            : null });
        }

        private void _deviceHandler_DeviceRemoved(object sender, Devices.Device e)
        {
            if (InvokeIfRequired(new EventHandler<PalmSens.Devices.Device>(DeviceHandlerDeviceDiscovered), e))
            {
                return;
            }

            DeviceRemoved?.Invoke(this, new PlatformDevice()
            {
                Name = e.ToString(), Device = e,
                DeviceID =
                    e is FTDIDevice ftdiDevice
                        ? (int?)ftdiDevice.UsbDevice.DeviceId
                        : e is UsbCdcDevice usbCdcDevice
                            ? (int?)usbCdcDevice.UsbDevice.DeviceId
                            : null
            });
        }

        public Task<CommManager> Connect(Devices.Device device)
        {
            return _deviceHandler.ConnectAsync(device);
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