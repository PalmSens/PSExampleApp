using MvvmHelpers;
using Plugin.Media;
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
        private readonly IShareService _shareService;
        private bool _isCreatingReport;
        private HeavyMetalMeasurement _loadedMeasurement;

        public MeasurementDataViewModel(IMeasurementService measurementService, IShareService shareService)
        {
            _measurementService = measurementService;
            _shareService = shareService;
            LoadedMeasurement = _measurementService.ActiveMeasurement;
            _popupNavigation = PopupNavigation.Instance;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);

            ShareMeasurementCommand = CommandFactory.Create(ShareMeasurement);
            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
            OnPhotoSelected = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            //OnPhotoSelected = CommandFactory.Create(OpenPhoto);
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
        }

        public bool IsCreatingReport
        {
            get => _isCreatingReport;
            set => SetProperty(ref _isCreatingReport, value);
        }

        public HeavyMetalMeasurement LoadedMeasurement
        {
            get => _loadedMeasurement;
            set => SetProperty(ref _loadedMeasurement, value);
        }

        public ObservableCollection<ImageSource> MeasurementPhotos { get; } = new ObservableCollection<ImageSource>();
        public ICommand OnPageAppearingCommand { get; }

        public ICommand OnPhotoSelected { get; }

        public ICommand ShareMeasurementCommand { get; }

        public ICommand ShowPlotCommand { get; }

        public ICommand TakePhotoCommand { get; }

        private void OnPageAppearing()
        {
            if (LoadedMeasurement.MeasurementImages == null)
                LoadedMeasurement.MeasurementImages = new List<byte[]>();

            foreach (var image in LoadedMeasurement.MeasurementImages)
            {
                MeasurementPhotos.Add(ImageSource.FromStream(() =>
                {
                    return new MemoryStream(image);
                }));
            }
        }

        private async Task OpenPhoto(ImageSource photo)
        {
            await _popupNavigation.PushAsync(new MeasurementPhotoPopup(photo));
        }

        private async Task ShareMeasurement()
        {
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, $"Report{LoadedMeasurement.Name}");
            IsCreatingReport = true;

            //CreatePDFfile is a long running proces that isn't async by itself.
            await Task.Run(() => _shareService.CreatePdfFile(LoadedMeasurement, cacheFile));

            IsCreatingReport = false;

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Sharing is Caring",
                File = new ShareFile(cacheFile)
            });
        }

        private async Task TakePhoto()
        {
            var mediaOptions = new MediaPickerOptions();

            //var result = await MediaPicker.CapturePhotoAsync();

            var result = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { CompressionQuality = 20, MaxWidthHeight = 200 }).ConfigureAwait(false);

            if (result != null)
            {
                var stream = result.GetStream();

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);

                    var byteArray = memoryStream.ToArray();
                    await _measurementService.SavePhoto(byteArray);

                    var image = ImageSource.FromStream(() => memoryStream);

                    MeasurementPhotos.Add(image);
                }
            }
        }
    }
}