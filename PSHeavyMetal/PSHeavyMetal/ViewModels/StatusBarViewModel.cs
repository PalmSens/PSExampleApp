using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Resx;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class StatusBarViewModel : BaseViewModel
    {
        private readonly IDeviceService _deviceService;
        private readonly IMeasurementService _measurementService;
        private readonly IMessageService _messageService;
        private bool _hasActiveMeasurement;
        private bool _isConnected;
        private IPermissionService _permissionService;
        private IPopupNavigation _popupNavigation;
        private string _statusText;

        public StatusBarViewModel(IDeviceService deviceService, IPermissionService permissionService, IMeasurementService measurementService, IMessageService messageService)
        {
            _messageService = messageService;
            _measurementService = measurementService;
            _permissionService = permissionService;
            _deviceService = deviceService;
            _popupNavigation = PopupNavigation.Instance;
            _deviceService.DeviceStateChanged += _deviceService_DeviceStateChanged;
            _measurementService.MeasurementChanged += _measurementService_MeasurementReset;

            OpenSettingsCommand = CommandFactory.Create(OpenSettings);
            OpenDataCommand = CommandFactory.Create(OpenData);
            OnViewAppearingCommand = CommandFactory.Create(OnViewAppearing, onException: ex =>
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                //DisplayAlert();
                                Console.WriteLine(ex.Message);
                            }), allowsMultipleExecutions: false);
        }

        public bool HasActiveMeasurement
        {
            get => _hasActiveMeasurement;
            set => SetProperty(ref _hasActiveMeasurement, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set => SetProperty(ref _isConnected, value);
        }

        public ICommand OnViewAppearingCommand { get; }

        public ICommand OpenDataCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public async Task OpenData()
        {
            await _popupNavigation.PushAsync(new SelectMeasurementPopup());
        }

        public async Task OpenSettings()
        {
            await _popupNavigation.PushAsync(new SettingsPopUp());
        }

        private void _deviceService_DeviceDiscovered(object sender, PalmSens.Core.Simplified.XF.Application.Models.PlatformDevice e)
        {
            if (_deviceService.AvailableDevices.Count == 1)
                StatusText = AppResources.StatusBarReaderFound;
            else
                StatusText = string.Format(AppResources.StatusBarMultipleReaders, _deviceService.AvailableDevices.Count);
        }

        private void _deviceService_DeviceStateChanged(object sender, Common.Models.DeviceState e)
        {
            switch (e)
            {
                case Common.Models.DeviceState.Connected:
                    _deviceService.DeviceDiscovered -= _deviceService_DeviceDiscovered;
                    StatusText = string.Format(AppResources.StatusBarConnected, _deviceService.ConnectedDevice.Name);
                    IsConnected = true;
                    break;

                case Common.Models.DeviceState.Disconnected:
                    StatusText = AppResources.StatusBarDisconnected;
                    IsConnected = false;
                    break;

                case Common.Models.DeviceState.Detecting:
                    _deviceService.DeviceDiscovered += _deviceService_DeviceDiscovered;
                    StatusText = AppResources.Searching;
                    IsConnected = false;
                    break;

                case Common.Models.DeviceState.Connecting:
                    StatusText = AppResources.StatusBarConnecting;
                    IsConnected = false;
                    break;

                default:
                    break;
            }
        }

        private void _measurementService_MeasurementReset(object sender, Common.Models.HeavyMetalMeasurement e)
        {
            if (e != null)
                HasActiveMeasurement = true;
            else
                HasActiveMeasurement = false;
        }

        private async Task DiscoverDevices()
        {
            _deviceService.DeviceDiscovered += _deviceService_DeviceDiscovered;

            try
            {
                await _deviceService.DetectDevicesAsync();
            }
            catch (PermissionException)
            {
                _messageService.ShortAlert("Please allow bluetooth persmission to start scanning");
                await _permissionService.RequestBluetoothPermission();
                await DiscoverDevices();
            }
            catch (Exception ex)
            {
                _messageService.LongAlert($"Discovering devices failed. Retrying to start the scanner. {ex}");
                await DiscoverDevices();
            }
        }

        private async Task OnViewAppearing()
        {
            //We only want to start detecting devices when it's not yet
            if (!_deviceService.IsDetecting && !_deviceService.IsConnected)
            {
                await _permissionService.RequestBluetoothPermission();                

                await DiscoverDevices();
            }
        }
    }
}