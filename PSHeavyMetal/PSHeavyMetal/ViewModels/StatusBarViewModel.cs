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
        private IPopupNavigation _popupNavigation;
        private string _statusText;

        public StatusBarViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
            _popupNavigation = PopupNavigation.Instance;
            _deviceService.DeviceStateChanged += _deviceService_DeviceStateChanged;

            OpenSettingsCommand = CommandFactory.Create(OpenSettings);
            OnViewAppearingCommand = CommandFactory.Create(OnViewAppearing, onException: ex =>
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                //DisplayAlert();
                                Console.WriteLine(ex.Message);
                            }), allowsMultipleExecutions: false);
        }

        public ICommand OnViewAppearingCommand { get; }

        public ICommand OpenSettingsCommand { get; }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
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
                    break;

                case Common.Models.DeviceState.Disconnected:
                    StatusText = "Disconnected";
                    break;

                case Common.Models.DeviceState.Detecting:
                    _deviceService.DeviceDiscovered += _deviceService_DeviceDiscovered;
                    StatusText = "Searching...";
                    break;

                case Common.Models.DeviceState.Connecting:
                    StatusText = "Connecting...";
                    break;

                default:
                    break;
            }
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