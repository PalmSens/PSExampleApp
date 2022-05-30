using PalmSens.Core.Simplified.XF.Application.Models;
using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IDeviceService
    {
        public event EventHandler<PlatformDevice> DeviceDiscovered;

        public event EventHandler<PlatformDevice> DeviceRemoved;

        public event EventHandler<DeviceState> DeviceStateChanged;

        /// <summary>
        /// The available devices that are discovered
        /// </summary>
        public List<PlatformDevice> AvailableDevices { get; }

        /// <summary>
        /// The device that the application is connected to
        /// </summary>
        public PlatformDevice ConnectedDevice { get; }

        public bool IsConnected { get; }
        public bool IsDetecting { get; }

        /// <summary>
        /// Connects to a device. This cancels the disovery process
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public Task ConnectToDeviceAsync(PlatformDevice device);

        /// <summary>
        /// Detects devices. This is a continous process until it gets cancelled by the connect method
        /// </summary>
        /// <returns></returns>
        public Task DetectDevicesAsync();
    }
}