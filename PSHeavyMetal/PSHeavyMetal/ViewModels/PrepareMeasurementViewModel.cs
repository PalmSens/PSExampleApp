using PalmSens.Core.Simplified.XF.Application.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
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
        private IMeasurementService _measurementService;
        private PlatformDevice _platformDevice;
        private string _sampleName;
        private string _sampleNotes;

        public ICommand ContinueCommand { get; }
        public ICommand OnPageAppearingCommand { get; }

        public PrepareMeasurementViewModel(IDeviceService deviceService, IMeasurementService measurementService)
        {
            _deviceService = deviceService;
            _measurementService = measurementService;

            ContinueCommand = CommandFactory.Create(async () => await Continue());
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException: ex =>
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    //DisplayAlert();
                    Console.WriteLine(ex.Message);
                }), allowsMultipleExecutions: false);
        }

        private async Task Continue()
        {
            _measurementService.CreateMeasurement(SampleName, SampleNotes);
            await NavigationDispatcher.Push(NavigationViewType.ConfigureMeasurementView);
        }

        public string SampleName
        {
            get => _sampleName;
            set => SetProperty(ref _sampleName, value);
        }

        public string SampleNotes
        {
            get => _sampleNotes;
            set => SetProperty(ref _sampleNotes, value);
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