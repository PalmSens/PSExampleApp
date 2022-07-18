using PalmSens.Core.Simplified.XF.Application.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface IInstrumentPlatform : IPlatform, IDisposable
    {
        event EventHandler<PlatformDevice> DeviceDiscovered;

        Task<List<PlatformDevice>> GetConnectedDevices(CancellationToken? cancellationToken = null);
    }
}