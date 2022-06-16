using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);

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

        public ObservableCollection<byte[]> MeasurementPhotos { get; } = new ObservableCollection<byte[]>();

        public ICommand OnPageAppearingCommand { get; }

        public ICommand OnPhotoSelected { get; }

        public ICommand ShowPlotCommand { get; }

        public ICommand TakePhotoCommand { get; }

        private void OnPageAppearing()
        {
            if (LoadedMeasurement.MeasurementImages == null)
                LoadedMeasurement.MeasurementImages = new List<byte[]>();

            foreach (var image in LoadedMeasurement.MeasurementImages)
            {
                MeasurementPhotos.Add(image);
                
            }
        }

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

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);

                    var byteArray = memoryStream.ToArray();
                    await _measurementService.SavePhoto(byteArray);

                    //var image = ImageSource.FromStream(() => memoryStream);
                    //MeasurementPhotos.Add(image);
                }
            }
        }
    }
}