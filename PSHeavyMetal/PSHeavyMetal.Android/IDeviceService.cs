using PalmSens.Core.Simplified.XF.Application.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IDeviceService
    {
        public PlatformDevice ConnectedDevice { get; }

        public Task DetectDevicesAsync(CancellationToken? cancellationToken = null);

        public Task ConnectToDeviceAsync(PlatformDevice device);

        public event EventHandler<PlatformDevice> DeviceDiscovered;
    }
}