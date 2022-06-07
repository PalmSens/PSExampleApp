using MvvmHelpers;
using PSHeavyMetal.Core.Services;
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
        private readonly IUserService _userService;
        private bool _hasActiveUser;
        private bool _isConnected;
        private IPopupNavigation _popupNavigation;
        private string _statusText;

        public StatusBarViewModel(IDeviceService deviceService, IUserService userService)
        {
            _deviceService = deviceService;
            _popupNavigation = PopupNavigation.Instance;
            _deviceService.DeviceStateChanged += _deviceService_DeviceStateChanged;

            OpenSettingsCommand = CommandFactory.Create(OpenSettings);
            OpenDataCommand = CommandFactory.Create(OpenData);
            OnViewAppearingCommand = CommandFactory.Create(OnViewAppearing, onException: ex =>
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                //DisplayAlert();
                                Console.WriteLine(ex.Message);
                            }), allowsMultipleExecutions: false);
            _userService = userService;

            _userService.ActiveUserChanged += _userService_ActiveUserChanged;
        }

        public bool HasActiveUser
        {
            get => _hasActiveUser;
            set => SetProperty(ref _hasActiveUser, value);
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
                StatusText = "1 reader found";
            else
                StatusText = $"{_deviceService.AvailableDevices.Count} readers found";
        }

        private void _deviceService_DeviceStateChanged(object sender, Common.Models.DeviceState e)
        {
            switch (e)
            {
                case Common.Models.DeviceState.Connected:
                    _deviceService.DeviceDiscovered -= _deviceService_DeviceDiscovered;
                    StatusText = $"Connected to {_deviceService.ConnectedDevice.Name}";
                    IsConnected = true;
                    break;

                case Common.Models.DeviceState.Disconnected:
                    StatusText = "Disconnected";
                    IsConnected = false;
                    break;

                case Common.Models.DeviceState.Detecting:
                    _deviceService.DeviceDiscovered += _deviceService_DeviceDiscovered;
                    StatusText = "Searching";
                    IsConnected = false;
                    break;

                case Common.Models.DeviceState.Connecting:
                    StatusText = "Connecting";
                    IsConnected = false;
                    break;

                default:
                    break;
            }
        }

        private void _userService_ActiveUserChanged(object sender, Common.Models.User e)
        {
            if (e != null)
                HasActiveUser = true;
            else
                HasActiveUser = false;
        }

        private async Task DiscoverDevices()
        {
            _deviceService.DeviceDiscovered += _deviceService_DeviceDiscovered;
            await _deviceService.DetectDevicesAsync();
        }

        private async Task OnViewAppearing()
        {
            //We only want to start detecting devices when it's not yet
            if (!_deviceService.IsDetecting && !_deviceService.IsConnected)
                await DiscoverDevices();
        }
    }
}