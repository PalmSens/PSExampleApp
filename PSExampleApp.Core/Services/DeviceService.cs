using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Extentions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PSExampleApp.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly InstrumentService _instrumentService;
        private CancellationTokenSource _cancellationTokenSource;

        private System.Timers.Timer _timer;

        public DeviceService(InstrumentService instrumentService)
        {
            _instrumentService = instrumentService;
        }

        public event EventHandler<Exception> DeviceDisconnected;

        public event EventHandler<PlatformDevice> DeviceDiscovered;

        public event EventHandler<PlatformDevice> DeviceRemoved;

        public event EventHandler<DeviceState> DeviceStateChanged;

        public List<PlatformDevice> AvailableDevices { get; } = new List<PlatformDevice>();

        public PlatformDevice ConnectedDevice { get; private set; }

        public bool IsConnected => this.ConnectedDevice != null;

        public bool IsDetecting { get; private set; }

        public async Task ConnectToDeviceAsync(PlatformDevice device)
        {
            DeviceStateChanged.Invoke(this, DeviceState.Connecting);
            _cancellationTokenSource.Cancel();

            _instrumentService.DeviceDiscovered -= _instrumentService_DeviceDiscovered;

            IsDetecting = false;

            await Task.Delay(10);
            _cancellationTokenSource.Dispose();

            try
            {
                await _instrumentService.ConnectAsync(device.Device).ConfigureAwait(false);
                ConnectedDevice = device;
                DeviceStateChanged.Invoke(this, DeviceState.Connected);
                _instrumentService.Disconnected += _instrumentService_Disconnected;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Connection failed {ex}");
                throw;
            }
        }

        public async Task DetectDevicesAsync()
        {
            DeviceStateChanged.Invoke(this, DeviceState.Detecting);
            AvailableDevices.Clear();
            _instrumentService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            _instrumentService.DeviceRemoved += _instrumentService_DeviceRemoved;

            _cancellationTokenSource = new CancellationTokenSource();
            IsDetecting = true;

            _timer = new System.Timers.Timer
            {
                Enabled = true,
                Interval = 5000,
            };

            _timer.Elapsed += _timer_Elapsed;

            try
            {
                await _instrumentService.GetConnectedDevices(_cancellationTokenSource.Token).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        /// <summary>
        /// Disconnect the device and restarts device discovery
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectDevice()
        {
            try
            {
                await _instrumentService.DisconnectAsync();
                this.ConnectedDevice = null;
                this._instrumentService.Disconnected -= _instrumentService_Disconnected;
            }
            catch (NullReferenceException ex)
            {
                // With a null exception it means its all ready disconnected. We don't want to do anything except log the exception and force invoke the disconnected event.
                Debug.WriteLine(ex);
                this.DeviceDisconnected?.Invoke(this, ex);
                this.ConnectedDevice = null;
            }

            await this.DetectDevicesAsync();
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            if (!AvailableDevices.Contains(e))
            {
                AvailableDevices.Add(e);
                DeviceDiscovered?.Invoke(this, e);
            }
        }

        private void _instrumentService_DeviceRemoved(object sender, PlatformDevice e)
        {
            var deviceToBeRemoved = AvailableDevices.FirstOrDefault(x => x.Name == e.Name);

            if (deviceToBeRemoved != null)
            {
                AvailableDevices.Remove(deviceToBeRemoved);
                DeviceRemoved?.Invoke(this, e);
            }

            if (ConnectedDevice.Name == e.Name)
            {
                ConnectedDevice = null;
                DeviceStateChanged?.Invoke(this, DeviceState.Disconnected);
            }
        }

        private void _instrumentService_Disconnected(object sender, Exception CommErrorException)
        {
            this.DeviceDisconnected?.Invoke(this, CommErrorException);
            this.DeviceStateChanged?.Invoke(this, DeviceState.Disconnected);
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //If the timing is triggered and no instruments are found then the application should restart the scanning.
            if (AvailableDevices.Count == 0)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;

                _cancellationTokenSource.Cancel();
                Task.Delay(10);
                _cancellationTokenSource.Dispose();

                _instrumentService.DeviceDiscovered -= _instrumentService_DeviceDiscovered;
                _instrumentService.DeviceRemoved -= _instrumentService_DeviceRemoved;

                DetectDevicesAsync().WithCallback();
            }
        }
    }
}