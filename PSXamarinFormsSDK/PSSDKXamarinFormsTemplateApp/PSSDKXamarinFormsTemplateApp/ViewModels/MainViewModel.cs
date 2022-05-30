using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Techniques;
using PSSDKXamarinFormsTemplateApp.Pages;
using PSSDKXamarinFormsTemplateApp.Services;
using PSSDKXamarinFormsTemplateApp.Views;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        private string _currentView;
        public ICommand OnPageAppearingCommand { get; }
        private CancellationTokenSource _instrumentDiscoveryCancellationTokenSource;
        private IPermissionService _permissionService;
        private string _status;
        public ICommand OnInstrumentSelected { get; }
        private volatile bool _connecting = false;

        public string CurrentView
        {
            get => _currentView;
            set => SetProperty(ref _currentView, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public MainViewModel()
        {
            _instrumentDiscoveryCancellationTokenSource = new CancellationTokenSource(); //Add optional timeout
            InstrumentService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException: ex =>
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert();
                    Console.WriteLine(ex.Message);
                }), allowsMultipleExecutions: false);
            //OnPageDisappearingCommand = CommandFactory.Create(AbortDeviceDiscovery);
            OnInstrumentSelected = CommandFactory.Create(async pd => await ConnectToInstrument(pd as PlatformDevice));
            _permissionService = DependencyService.Resolve<IPermissionService>();
            CurrentView = nameof(ConnectionView);

            DependencyService.RegisterSingleton(this);
        }

        private async Task DiscoverDevices()
        {
            Status = "Scanning...";
            await InstrumentService.GetConnectedDevices(_instrumentDiscoveryCancellationTokenSource.Token);
            Status = "Scan complete.";
        }

        private async Task OnPageAppearing()
        {
            await _permissionService.RequestBluetoothPermission();
            await DiscoverDevices();
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {

        }

        private void AbortDeviceDiscovery()
        {
            _instrumentDiscoveryCancellationTokenSource.Cancel();
        }

        private async Task ConnectToInstrument(PlatformDevice device)
        {
            if(_connecting) return;
            _connecting = true;
            Status = "Cancelling scan...";
            _instrumentDiscoveryCancellationTokenSource.Cancel();
            await Task.Delay(10);
            Status = "Connecting...";
            Task connectTask = InstrumentService.ConnectAsync(device.Device);
            CurrentView = nameof(MeasurementView);
            await connectTask;
            Status = "Connected";
            await InstrumentService.MeasureAsync(new AmperometricDetection()
            {
                ConditioningTime = 0,
                DepositionTime = 0,
                EquilibrationTime = 1,
                RunTime = 3
            });
            await InstrumentService.DisconnectAsync();
            _instrumentDiscoveryCancellationTokenSource.Dispose();
            _instrumentDiscoveryCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var token = _instrumentDiscoveryCancellationTokenSource.Token;
            var devices = await InstrumentService.GetConnectedDevices(token);
        }
    }
}