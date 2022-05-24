using Android.Content;
using PalmSens.Comm;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Devices;
using PalmSens.PSAndroid.Comm;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PalmSens.Core.Simplified.XF.Infrastructure.Android.Services
{
    internal class DeviceHandler : IDeviceHandler
    {
        internal Context Context;

        internal DeviceHandler()
        {
        }

        public event EventHandler<Device> DeviceDiscoverered;

        public bool EnableBluetooth { get; set; } = true;
        public bool EnableUSB { get; set; } = true;

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
                //device.Open();
                //comm = new CommManager(device);

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
            DeviceDiscoverered = null;
        }

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
            DeviceDiscoverer deviceDiscoverer = null;

            try //Attempt to find connected palmsens/emstat devices
            {
                deviceDiscoverer = new DeviceDiscoverer(Context);
                deviceDiscoverer.DeviceDiscovered += DeviceDiscoverer_DeviceDiscovered;
                devices = (await deviceDiscoverer.Discover(EnableUSB, EnableBluetooth, cancellationToken, timeOut))
                    .ToArray();
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    $"An error occured while attempting to scan for connected devices. {ex.Message}");
            }
            finally
            {
                if (deviceDiscoverer != null)
                {
                    deviceDiscoverer.DeviceDiscovered -= DeviceDiscoverer_DeviceDiscovered;
                    deviceDiscoverer.Dispose();
                }
            }
            return devices;
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
        [Obsolete("Compatible with SDKs 5.4 and earlier. Please use asynchronous functions, as development of synchronous functions will be fased out")]
        internal CommManager ConnectBC(Device device)
        {
            if (device == null)
                throw new ArgumentNullException("The specified device cannot be null.");
            CommManager comm = null;

            try
            {
                device.Open(); //Open the device to allow a connection
                comm = new CommManager(device); //Connect to the selected device
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

        private void DeviceDiscoverer_DeviceDiscovered(object sender, Device e)
        {
            DeviceDiscoverered?.Invoke(this, e);
        }
    }
}