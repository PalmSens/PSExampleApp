using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PalmSens.Comm;
using PalmSens.Devices;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface IDeviceHandler: IDisposable
    {
        bool EnableBluetooth { get; set; }
        bool EnableUSB { get; set; }
        Task<Device[]> ScanDevicesAsync(CancellationToken? cancellationToken = null, int timeOut = 20000);
        Task<CommManager> ConnectAsync(Device device);
        Task DisconnectAsync(CommManager comm);
        event EventHandler<Device> DeviceDiscoverered;
    }
}
