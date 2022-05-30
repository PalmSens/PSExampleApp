using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly InstrumentService _instrumentService;
        private CancellationTokenSource _cancellationTokenSource;

        public DeviceService(InstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        public event EventHandler<PlatformDevice> DeviceDiscovered;

        public event EventHandler<PlatformDevice> DeviceRemoved;

        public event EventHandler<DeviceState> DeviceStateChanged;

        public List<PlatformDevice> AvailableDevices { get; } = new List<PlatformDevice>();

        public PlatformDevice ConnectedDevice { get; private set; }

        public bool IsConnected => this.ConnectedDevice != null;

        public bool IsDetecting { get; private set; }

        public async Task ConnectToDeviceAsync(PlatformDevice device)
        {
            DeviceStateChanged.Invoke(this, DeviceState.Connecting);
            _cancellationTokenSource.Cancel();

            _instrumentService.DeviceDiscovered -= _instrumentService_DeviceDiscovered;

            IsDetecting = false;

            await Task.Delay(10);
            _cancellationTokenSource.Dispose();

            try
            {
                await _instrumentService.ConnectAsync(device.Device).ConfigureAwait(false);
                ConnectedDevice = device;
                DeviceStateChanged.Invoke(this, DeviceState.Connected);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection failed {ex}");
                await DetectDevicesAsync();
            }
        }

        public async Task DetectDevicesAsync()
        {
            DeviceStateChanged.Invoke(this, DeviceState.Detecting);
            AvailableDevices.Clear();
            _instrumentService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            _instrumentService.DeviceRemoved += _instrumentService_DeviceRemoved;

            _cancellationTokenSource = new CancellationTokenSource();
            IsDetecting = true;
            await _instrumentService.GetConnectedDevices(_cancellationTokenSource.Token).ConfigureAwait(false);
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            if (!AvailableDevices.Contains(e))
            {
                AvailableDevices.Add(e);
                DeviceDiscovered?.Invoke(this, e);
            }
        }

        private void _instrumentService_DeviceRemoved(object sender, PlatformDevice e)
        {
            var deviceToBeRemoved = AvailableDevices.FirstOrDefault(x => x.Name == e.Name);

            if (deviceToBeRemoved != null)
            {
                AvailableDevices.Remove(deviceToBeRemoved);
                DeviceRemoved?.Invoke(this, e);
            }

            if (ConnectedDevice.Name == e.Name)
            {
                ConnectedDevice = null;
                DeviceStateChanged?.Invoke(this, DeviceState.Disconnected);
            }
        }
    }
}