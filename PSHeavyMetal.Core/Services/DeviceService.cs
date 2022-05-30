using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly InstrumentService _instrumentService;

        public DeviceService(InstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        public event EventHandler<PlatformDevice> DeviceDiscovered;

        public event EventHandler<DeviceState> DeviceStateChanged;

        public List<PlatformDevice> AvailableDevices { get; } = new List<PlatformDevice>();

        public PlatformDevice ConnectedDevice { get; private set; }

        public bool IsConnected => this.ConnectedDevice != null;

        public bool IsDetecting { get; private set; }

        public async Task ConnectToDeviceAsync(PlatformDevice device)
        {
            DeviceStateChanged.Invoke(this, DeviceState.Connecting);
            _instrumentService.DeviceDiscovered -= _instrumentService_DeviceDiscovered;
            IsDetecting = false;
            await _instrumentService.ConnectAsync(device.Device).ConfigureAwait(false);
            ConnectedDevice = device;
            DeviceStateChanged.Invoke(this, DeviceState.Connected);
        }

        public async Task DetectDevicesAsync(CancellationToken? cancellationToken = null)
        {
            DeviceStateChanged.Invoke(this, DeviceState.Detecting);
            AvailableDevices.Clear();
            _instrumentService.DeviceDiscovered += _instrumentService_DeviceDiscovered;

            IsDetecting = true;
            await _instrumentService.GetConnectedDevices().ConfigureAwait(false);
            System.Diagnostics.Debug.WriteLine("Done discovering");
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            AvailableDevices.Add(e);
            DeviceDiscovered?.Invoke(this, e);
        }
    }
}