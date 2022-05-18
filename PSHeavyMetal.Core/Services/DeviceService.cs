using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using System;
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

        public event EventHandler<PlatformDevice> DeviceDiscovered
        {
            add => _instrumentService.DeviceDiscovered += value;
            remove => _instrumentService.DeviceDiscovered -= value;
        }

        public PlatformDevice ConnectedDevice { get; private set; }

        public async Task ConnectToDeviceAsync(PlatformDevice device)
        {
            await _instrumentService.ConnectAsync(device.Device);
            ConnectedDevice = device;
        }

        public async Task DetectDevicesAsync(CancellationToken? cancellationToken = null)
        {
            await _instrumentService.GetConnectedDevices().ConfigureAwait(false);
        }
    }
}