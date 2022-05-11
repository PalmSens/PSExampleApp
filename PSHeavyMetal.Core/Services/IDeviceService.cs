using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Devices;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IDeviceService
    {
        public Task DetectDevicesAsync(CancellationToken? cancellationToken = null);

        public Task ConnectToDeviceAsync(Device device);

        public event EventHandler<PlatformDevice> DeviceDiscovered;
    }
}