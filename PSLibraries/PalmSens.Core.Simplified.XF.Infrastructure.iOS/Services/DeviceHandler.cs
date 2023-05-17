using System;
using System.Threading;
using System.Threading.Tasks;
using PalmSens.Comm;
using PalmSens.Core.iOS.Comm;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Devices;

namespace PalmSens.Core.Simplified.XF.Infrastructure.iOS.Services
{
    class DeviceHandler: IDeviceHandler
    {
        internal DeviceHandler()
        {
        }

        public bool EnableBluetooth { get; set; } = true;
        public bool EnableUSB { get; set; } = true;
        public event EventHandler<Device> DeviceDiscovered;
        public event EventHandler<Device> DeviceRemoved;

        /// <summary>
        /// Scans for connected devices.
        /// </summary>
        /// <param name="timeOut">Discovery time out in milliseconds.</param>
        /// <returns>
        /// Returns an array of connected devices
        /// </returns>
        /// <exception cref="System.ArgumentException">An error occured while attempting to scan for connected devices.</exception>
        public async Task<Device[]> ScanDevicesAsync(CancellationToken? cancellationToken = null, int timeOut = 20000)
        {
            Device[] devices = new Device[0];
            DiscoverDevices discoverDevices = null;
            if (cancellationToken is null)
            {
                throw new ArgumentNullException(nameof(cancellationToken));
            }

            try //Attempt to find connected palmsens/emstat devices
            {
                discoverDevices = new DiscoverDevices();
                discoverDevices.DeviceDiscovered += DeviceDiscoverer_DeviceDiscovered;
                devices = (await discoverDevices.Discover((CancellationToken)cancellationToken, timeOut))
                    .ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"An error occured while attempting to scan for connected devices. {ex.Message}");
            }
            finally
            {
                if(discoverDevices != null)
                {
                    discoverDevices.DeviceDiscovered -= DeviceDiscoverer_DeviceDiscovered;
                    discoverDevices.Dispose();
                }
            }
            return devices;
        }

        private void DeviceDiscoverer_DeviceDiscovered(Device e)
        {
            DeviceDiscovered?.Invoke(this, e);
        }

        /// <summary>
        /// Connects to the specified device and returns its CommManager.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>
        /// The CommManager of the device or null
        /// </returns>
        /// <exception cref="System.ArgumentNullException">The specified device cannot be null.</exception>
        /// <exception cref="System.Exception">Could not connect to the specified device.</exception>
        public async Task<CommManager> ConnectAsync(Device device)
        {
            if (device == null)
                throw new ArgumentNullException("The specified device cannot be null.");
            CommManager comm = null;

            await new SynchronizationContextRemover();

            try
            {
                await device.OpenAsync(); //Open the device to allow a connection
                comm = await CommManager.CommManagerAsync(device); //Connect to the selected device
            }
            catch (Exception ex)
            {
                device.Close();
                throw new Exception($"Could not connect to the specified device. {ex.Message}");
            }

            return comm;
        }

        /// <summary>
        /// Disconnects the device using its CommManager.
        /// </summary>
        /// <param name="comm">The device's CommManager.</param>
        /// <exception cref="System.ArgumentNullException">The specified CommManager cannot be null.</exception>
        internal void Disconnect(CommManager comm)
        {
            if (comm == null)
                throw new ArgumentNullException("The specified CommManager cannot be null.");
            comm.Disconnect();
        }

        /// <summary>
        /// The asynchronous version of method 'Disconnect'.
        /// </summary>
        /// <param name="comm">The device's CommManager.</param>
        /// <exception cref="System.ArgumentNullException">The specified CommManager cannot be null.</exception>
        public async Task DisconnectAsync(CommManager comm)
        {
            if (comm == null) throw new ArgumentNullException("The specified CommManager cannot be null.");
            await comm.DisconnectAsync();
        }

        public void Dispose()
        {
            DeviceDiscovered = null;
            DeviceRemoved = null;
        }
    }
}