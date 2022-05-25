using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PalmSens.Core.Simplified.XF.Application.Models;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface IInstrumentPlatfrom : IPlatform, IDisposable
    {
        Task<List<PlatformDevice>> GetConnectedDevices(CancellationToken? cancellationToken = null);
        event EventHandler<PlatformDevice> DeviceDiscovered;
    }
}
