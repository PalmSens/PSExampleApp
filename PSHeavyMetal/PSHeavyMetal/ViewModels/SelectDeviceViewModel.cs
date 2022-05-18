using PalmSens.Core.Simplified.XF.Application.Models;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class SelectDeviceViewModel : BaseViewModel
    {
        private readonly CancellationTokenSource _deviceDiscoveryCancellationTokenSource;
        private readonly IDeviceService _deviceService;
        private readonly IPermissionService _permissionService;
        private bool _isConnecting;

        public SelectDeviceViewModel(IDeviceService deviceService, IPermissionService permissionService)
        {
            _deviceService = deviceService;
            _permissionService = permissionService;

            AvailableDevices.CollectionChanged += AvailableDevices_CollectionChanged;

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
            CancelCommand = CommandFactory.Create(async () => await NavigationDispatcher.Pop());
        }

        public ObservableCollection<PlatformDevice> AvailableDevices { get; } = new ObservableCollection<PlatformDevice>();

        public ICommand CancelCommand { get; }

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
                        return "Searching";

                    case 1:
                        return "1 reader found,\n please select:";

                    default:
                        return $"{AvailableDevices.Count} readers found,\n please select:";
                }
            }
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            AvailableDevices.Add(e);
        }

        private void AbortDeviceDiscovery()
        {
            _deviceDiscoveryCancellationTokenSource.Cancel();
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
            await _deviceService.ConnectToDeviceAsync(device);

            await NavigationDispatcher.Push(NavigationViewType.PrepareMeasurementView);
        }

        private async Task DiscoverDevices()
        {
            await _deviceService.DetectDevicesAsync(_deviceDiscoveryCancellationTokenSource.Token);
        }

        private async Task OnPageAppearing()
        {
            IsConnecting = false;
            await _permissionService.RequestBluetoothPermission();
            await DiscoverDevices();
        }
    }
}