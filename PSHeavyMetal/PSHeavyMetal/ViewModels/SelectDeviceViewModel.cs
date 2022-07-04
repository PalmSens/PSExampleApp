using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Resx;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class SelectDeviceViewModel : BaseViewModel
    {
        private readonly IDeviceService _deviceService;
        private readonly IMessageService _messageService;
        private readonly IPermissionService _permissionService;
        private bool _isConnecting;

        public SelectDeviceViewModel(IDeviceService deviceService, IMessageService messageService, IPermissionService permissionService)
        {
            _deviceService = deviceService;
            _messageService = messageService;
            _permissionService = permissionService;

            AvailableDevices.CollectionChanged += AvailableDevices_CollectionChanged;
            _deviceService.DeviceStateChanged += _deviceService_DeviceStateChanged;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
            OnPageDisappearingCommand = CommandFactory.Create(OnPageDisappearing);
            OnInstrumentSelected = CommandFactory.Create(async pd => await ConnectToInstrument(pd as PlatformDevice));
            CancelCommand = CommandFactory.Create(async () => await NavigationDispatcher.Pop());
            ContinueCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.ConfigureMeasurementView));
            DisconnectCommand = CommandFactory.Create(Disconnect);
        }

        public ObservableCollection<PlatformDevice> AvailableDevices { get; } = new ObservableCollection<PlatformDevice>();

        public ICommand CancelCommand { get; }

        public ICommand ContinueCommand { get; }

        public ICommand DisconnectCommand { get; }

        public bool IsConnected => _deviceService.ConnectedDevice != null;

        public bool IsConnecting
        {
            get => _isConnecting;
            set => SetProperty(ref _isConnecting, value);
        }

        public ICommand OnInstrumentSelected { get; }

        public ICommand OnPageAppearingCommand { get; }

        public ICommand OnPageDisappearingCommand { get; }

        public string ReaderResult
        {
            get
            {
                switch (AvailableDevices.Count)
                {
                    case 0:
                        return AppResources.Searching;

                    case 1:
                        return AppResources.SelectPageReaderFound;

                    default:
                        return string.Format(AppResources.SelectPageMultipleReaders, AvailableDevices.Count);
                }
            }
        }

        private void _deviceService_DeviceRemoved(object sender, PlatformDevice e)
        {
            var deviceToBeRemoved = AvailableDevices.FirstOrDefault(x => x.Name == e.Name);

            if (deviceToBeRemoved != null)
                AvailableDevices.Remove(deviceToBeRemoved);
        }

        private void _deviceService_DeviceStateChanged(object sender, Common.Models.DeviceState e)
        {
            OnPropertyChanged(nameof(IsConnected));
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            if (!AvailableDevices.Contains(e))
                AvailableDevices.Add(e);
        }

        private void AbortDeviceDiscovery()
        {
            _deviceService.DeviceDiscovered -= _instrumentService_DeviceDiscovered;
        }

        private void AvailableDevices_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ReaderResult));
        }

        private async Task ConnectToInstrument(PlatformDevice device)
        {
            IsConnecting = true;
            AbortDeviceDiscovery();
            await Task.Delay(100);

            try
            {
                await _deviceService.ConnectToDeviceAsync(device);
            }
            catch (Exception)
            {
                _messageService.LongAlert("Connection to device failed. Please try again");
                await ResetDeviceDiscovery();
            }

            await NavigationDispatcher.Push(NavigationViewType.ConfigureMeasurementView);
        }

        private async Task Disconnect()
        {
            await _deviceService.DisconnectDevice();
        }

        private void OnPageAppearing()
        {
            this.IsConnecting = false;

            foreach (var device in this._deviceService.AvailableDevices)
                this.AvailableDevices.Add(device);

            this._deviceService.DeviceDiscovered += this._instrumentService_DeviceDiscovered;
            this._deviceService.DeviceRemoved += this._deviceService_DeviceRemoved;
        }

        private void OnPageDisappearing()
        {
            AvailableDevices.CollectionChanged -= AvailableDevices_CollectionChanged;
            _deviceService.DeviceStateChanged -= _deviceService_DeviceStateChanged;
            AbortDeviceDiscovery();
        }

        /// <summary>
        /// This methods resets the available devices and tries to discover devices again
        /// </summary>
        /// <returns></returns>
        private async Task ResetDeviceDiscovery()
        {
            IsConnecting = false;
            AvailableDevices.Clear();
            _deviceService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            try
            {
                await _deviceService.DetectDevicesAsync();
            }
            catch (PermissionException)
            {
                _messageService.ShortAlert("Please allow bluetooth persmission to start scanning");
                await _permissionService.RequestBluetoothPermission();
                await _deviceService.DetectDevicesAsync();
            }
            catch (Exception ex)
            {
                _messageService.LongAlert($"Discovering devices failed. Retrying to start the scanner. {ex}");
                await _deviceService.DetectDevicesAsync();
            }
        }
    }
}