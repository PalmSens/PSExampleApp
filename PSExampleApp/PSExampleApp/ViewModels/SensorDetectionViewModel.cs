using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class SensorDetectionViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private bool _isSensorInserted;
        private string _sourcePath;

        public SensorDetectionViewModel(IMeasurementService measurementService)
        {
            _popupNavigation = PopupNavigation.Instance;
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;
            ContinueCommand = CommandFactory.Create(Continue);
            CancelCommand = CommandFactory.Create(Cancel);
            ConfirmSensorCommand = CommandFactory.Create(ConfirmSensor);

            MediaSourcePath = "ms-appx:///InsertSensor.mp4";
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public ICommand CancelCommand { get; }

        public ICommand ConfirmSensorCommand { get; }

        public ICommand ContinueCommand { get; }

        public bool IsSensorInserted
        {
            get => _isSensorInserted;
            set => SetProperty(ref _isSensorInserted, value);
        }

        /// <summary>
        /// This property is a string to the path of the media. It's done this way since changing the image source through xaml or a converter somehow breaks the media element
        /// Doing it this way seems to work.
        /// </summary>
        public string MediaSourcePath
        {
            get => _sourcePath;
            set => SetProperty(ref _sourcePath, value);
        }

        public void ConfirmSensor()
        {
            IsSensorInserted = true;
            MediaSourcePath = "ms-appx:///AddDroplet.mp4";
        }

        private async Task Cancel()
        {
            await _popupNavigation.PopAllAsync();
        }

        private async Task Continue()
        {
            await NavigationDispatcher.Push(NavigationViewType.RunMeasurementView);
            await _popupNavigation.PopAllAsync();
        }
    }
}