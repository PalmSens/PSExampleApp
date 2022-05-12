using PalmSens.Core.Simplified.XF.Application.Models;
using PSHeavyMetal.Core.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class PrepareMeasurementViewModel : BaseViewModel
    {
        private IDeviceService _deviceService;
        private PlatformDevice _platformDevice;
        public ICommand OnPageAppearingCommand { get; }

        public PrepareMeasurementViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException: ex =>
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert();
                    Console.WriteLine(ex.Message);
                }), allowsMultipleExecutions: false);
        }

        public PlatformDevice ConnectedDevice
        {
            get => _platformDevice;
            private set => SetProperty(ref _platformDevice, value);
        }

        private async Task OnPageAppearing()
        {
            ConnectedDevice = _deviceService.ConnectedDevice;
        }
    }
}