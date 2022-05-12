using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Views;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class SelectDeviceViewModel : BaseViewModel
    {
        public ICommand OnPageAppearingCommand { get; }
        public ICommand OnPageDisappearingCommand { get; }
        public ICommand OnInstrumentSelected { get; }

        private readonly IDeviceService _deviceService;
        private readonly IPermissionService _permissionService;
        private readonly CancellationTokenSource _deviceDiscoveryCancellationTokenSource;

        public SelectDeviceViewModel(IDeviceService deviceService, IPermissionService permissionService)
        {
            _deviceService = deviceService;
            _permissionService = permissionService;

            _deviceDiscoveryCancellationTokenSource = new CancellationTokenSource(); //Add optional timeout
            _deviceService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException: ex =>
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                //DisplayAlert();
                                Console.WriteLine(ex.Message);
                            }), allowsMultipleExecutions: false);
            OnPageDisappearingCommand = CommandFactory.Create(AbortDeviceDiscovery);
            OnInstrumentSelected = CommandFactory.Create(async pd => await ConnectToInstrument(pd as PlatformDevice));
        }

        public ObservableCollection<PlatformDevice> AvailableDevices { get; } = new ObservableCollection<PlatformDevice>();

        private async Task OnPageAppearing()
        {
            await _permissionService.RequestBluetoothPermission();
            await DiscoverDevices();
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            AvailableDevices.Add(e);
        }

        private async Task DiscoverDevices()
        {
            await _deviceService.DetectDevicesAsync(_deviceDiscoveryCancellationTokenSource.Token);
        }

        private async Task ConnectToInstrument(PlatformDevice device)
        {
            AbortDeviceDiscovery();
            await Task.Delay(100);
            await _deviceService.ConnectToDeviceAsync(device);

            await Shell.Current.GoToAsync($"//{nameof(PrepareMeasurementView)}");
        }

        private void AbortDeviceDiscovery()
        {
            _deviceDiscoveryCancellationTokenSource.Cancel();
            _deviceService.DeviceDiscovered -= _instrumentService_DeviceDiscovered;
        }
    }
}