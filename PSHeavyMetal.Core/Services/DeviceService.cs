using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Devices;
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

        public async Task DetectDevicesAsync(CancellationToken? cancellationToken = null)
        {
            await _instrumentService.GetConnectedDevices().ConfigureAwait(false);
        }

        public event EventHandler<PlatformDevice> DeviceDiscovered
        {
            add => _instrumentService.DeviceDiscovered += value;
            remove => _instrumentService.DeviceDiscovered -= value;
        }

        public async Task ConnectToDeviceAsync(Device device)
        {
            await _instrumentService.ConnectAsync(device);
        }
    }
}