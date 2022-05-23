using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PalmSens.Core.Simplified.XF.Application.Models;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public class InstrumentService : PSCommSimple
    {
        private readonly IInstrumentPlatform _platformDeviceManager;

        public InstrumentService(IInstrumentPlatform platform) : base(platform)
        {
            _platformDeviceManager = platform;
        }

        public Task<List<PlatformDevice>> GetConnectedDevices(CancellationToken? cancellationToken = null)
        {
            return _platformDeviceManager.GetConnectedDevices(cancellationToken);
        }

        public event EventHandler<PlatformDevice> DeviceDiscovered
        {
            add => _platformDeviceManager.DeviceDiscovered += value;
            remove => _platformDeviceManager.DeviceDiscovered -= value;
        }
    }
}