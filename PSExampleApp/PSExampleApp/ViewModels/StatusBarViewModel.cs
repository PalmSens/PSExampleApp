using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resx;
using PSExampleApp.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSExampleApp.Forms.ViewModels
{
    public class StatusBarViewModel : BaseAppViewModel
    {
        private readonly IDeviceService _deviceService;
        private readonly IMeasurementService _measurementService;
        private readonly IMessageService _messageService;
        private bool _hasActiveMeasurement;
        private bool _isConnected;
        private IPermissionService _permissionService;
        private IPopupNavigation _popupNavigation;
        private IUserService _userService;
        private string _statusText;

        public StatusBarViewModel(
            IDeviceService deviceService, 
            IPermissionService permissionService, 
            IMeasurementService measurementService, 
            IMessageService messageService,
            IUserService userService,
            IAppConfigurationService appConfigurationService) : base(appConfigurationService)
        {
            _messageService = messageService;
            _measurementService = measurementService;
            _permissionService = permissionService;
            _deviceService = deviceService;
            _userService = userService;
            _popupNavigation = PopupNavigation.Instance;
            _deviceService.DeviceStateChanged += _deviceService_DeviceStateChanged;
            _measurementService.MeasurementChanged += _measurementService_MeasurementReset;

            OpenSettingsCommand = CommandFactory.Create(OpenSettings);
            OpenDataCommand = CommandFactory.Create(OpenData);

            MessagingCenter.Subscribe<object>(this, "DiscoverDevices" , async (_) => { await GetBlueToothDevices(); });
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

        public ICommand OpenDataCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }

        public async Task OpenData()
        {
            if (_userService.ActiveUser != null)
            {
                await NavigationDispatcher.Push(NavigationViewType.SelectMeasurementView);
            }
        }

        public async Task OpenSettings()
        {
            if (_userService.ActiveUser != null)
            {
                await NavigationDispatcher.Push(NavigationViewType.SettingsView);
            }
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
                _messageService.ShortAlert(AppResources.Alert_AllowBluetooth);
                await _permissionService.RequestBluetoothPermission();
                await DiscoverDevices();
            }
            catch (Exception ex)
            {
                _messageService.LongAlert($"{AppResources.Alert_DiscoverFailed} {ex}");
                await DiscoverDevices();
            }
        }

        private async Task GetBlueToothDevices()
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