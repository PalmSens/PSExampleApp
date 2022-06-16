using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class MeasurementDataViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private HeavyMetalMeasurement _loadedMeasurement;

        public MeasurementDataViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            LoadedMeasurement = _measurementService.ActiveMeasurement;
            _popupNavigation = PopupNavigation.Instance;

            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
            OnPhotoSelected = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            //OnPhotoSelected = CommandFactory.Create(OpenPhoto);
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
        }

        public HeavyMetalMeasurement LoadedMeasurement
        {
            get => _loadedMeasurement;
            set => SetProperty(ref _loadedMeasurement, value);
        }

        public ObservableCollection<ImageSource> MeasurementPhotos { get; } = new ObservableCollection<ImageSource>();

        public ICommand OnPhotoSelected { get; }

        public ICommand ShowPlotCommand { get; }
        public ICommand TakePhotoCommand { get; }

        private async Task OpenPhoto(ImageSource photo)
        {
            await _popupNavigation.PushAsync(new MeasurementPhotoPopup(photo));
        }

        private async Task TakePhoto()
        {
            var result = await MediaPicker.CapturePhotoAsync();

            if (result != null)
            {
                var stream = await result.OpenReadAsync();

                var image = ImageSource.FromStream(() => stream);

                MeasurementPhotos.Add(image);
            }
        }
    }
}