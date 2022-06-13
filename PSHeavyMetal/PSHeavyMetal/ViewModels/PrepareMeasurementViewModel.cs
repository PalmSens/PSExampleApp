using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class PrepareMeasurementViewModel : BaseViewModel
    {
        private readonly IPopupNavigation _popupNavigation;
        private IMeasurementService _measurementService;
        private PlatformDevice _platformDevice;
        private string _sampleName;
        private string _sampleNotes;

        public PrepareMeasurementViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            _popupNavigation = PopupNavigation.Instance;
            ContinueCommand = CommandFactory.Create(async () => await Continue());
        }

        public PlatformDevice ConnectedDevice
        {
            get => _platformDevice;
            private set => SetProperty(ref _platformDevice, value);
        }

        public ICommand ContinueCommand { get; }

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

        private async Task Continue()
        {
            _measurementService.ActiveMeasurement.Name = SampleName;
            _measurementService.ActiveMeasurement.Configuration.Description = SampleNotes;

            await _popupNavigation.PushAsync(new SensorDetectionPopup());
        }
    }
}