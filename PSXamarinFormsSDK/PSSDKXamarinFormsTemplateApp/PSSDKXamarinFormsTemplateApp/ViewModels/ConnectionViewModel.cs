using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSSDKXamarinFormsTemplateApp.Pages;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.ViewModels
{
    internal class ConnectionViewModel : BaseViewModel
    {
        public ICommand OnPageAppearingCommand { get; }
        public ICommand OnPageDisappearingCommand { get; }
        public ICommand OnInstrumentSelected { get; }
        private readonly CancellationTokenSource _instrumentDiscoveryCancellationTokenSource;
        private IPermissionService _permissionService;

        public ObservableCollection<PlatformDevice> AvailableDevices { get; } =
            new ObservableCollection<PlatformDevice>();

        public ConnectionViewModel()
        {
            _permissionService = DependencyService.Resolve<IPermissionService>();
            _instrumentDiscoveryCancellationTokenSource = new CancellationTokenSource(); //Add optional timeout
            _instrumentService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException:ex =>
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert();
                    Console.WriteLine(ex.Message);
                }), allowsMultipleExecutions:false);
            OnPageDisappearingCommand = CommandFactory.Create(AbortDeviceDiscovery);
            OnInstrumentSelected = CommandFactory.Create(async pd => await ConnectToInstrument(pd as PlatformDevice));
        }

        private async Task OnPageAppearing()
        {
            await _permissionService.RequestBluetoothPermission();
            await DiscoverDevices();
        }

        private async Task DiscoverDevices()
        {
            await _instrumentService.GetConnectedDevices(_instrumentDiscoveryCancellationTokenSource.Token);
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            AvailableDevices.Add(e);
        }

        private async Task ConnectToInstrument(PlatformDevice device)
        {
            AbortDeviceDiscovery();
            await Task.Delay(100);
            await _instrumentService.ConnectAsync(device.Device);
            await App.Current.MainPage.Navigation.PushAsync(new MeasurementPage());
        }

        private void AbortDeviceDiscovery()
        {
            _instrumentDiscoveryCancellationTokenSource.Cancel();
        }
    }
}
