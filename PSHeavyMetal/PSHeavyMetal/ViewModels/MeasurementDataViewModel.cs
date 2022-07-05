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
using System.Diagnostics;
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

        //private bool _hasMax
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
            OnPhotoSelectedCommand = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
        }

        /// <summary>
        /// Gets if the measurement has a maximum amount of photos. This is 3
        /// </summary>
        public bool HasMaxPhotos => MeasurementPhotos.Count == 3;

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

        public ObservableCollection<MeasurementPhotoPresenter> MeasurementPhotos { get; } = new ObservableCollection<MeasurementPhotoPresenter>();

        public ICommand OnPageAppearingCommand { get; }

        public ICommand OnPageDisappearingCommand { get; }

        public ICommand OnPhotoSelectedCommand { get; }

        public ICommand ShareMeasurementCommand { get; }

        public ICommand ShowPlotCommand { get; }

        public ICommand TakePhotoCommand { get; }

        private void LoadPhoto(byte[] image)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var photo = new MeasurementPhotoPresenter
                {
                    Photo = ImageSource.FromStream(() =>
                    {
                        return new MemoryStream(image);
                    })
                };

                MeasurementPhotos.Add(photo);
            });
        }

        private void MeasurementPhotos_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(MeasurementPhotos));
            this.OnPropertyChanged(nameof(HasMaxPhotos));
        }

        private void OnDisappearing()
        {
            MeasurementPhotos.CollectionChanged -= MeasurementPhotos_CollectionChanged;
        }

        private async Task OnPageAppearing()
        {
            MeasurementPhotos.CollectionChanged += MeasurementPhotos_CollectionChanged;

            if (MeasurementPhotos.Count > 0)
                return;

            try
            {
                if (LoadedMeasurement.MeasurementImages == null)
                    LoadedMeasurement.MeasurementImages = new List<byte[]>();

                //We put adding the photos on a different thread since this can be a cpu heavy
                await Task.Run(() =>
                {
                    foreach (var image in LoadedMeasurement.MeasurementImages)
                    {
                        LoadPhoto(image);
                    }
                });
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
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
            var stream = await TakePictureAsync();

            var memoryStream = new MemoryStream();

            stream.CopyTo(memoryStream);
            stream.Dispose();

            var byteArray = memoryStream.ToArray();
            LoadedMeasurement.MeasurementImages.Add(byteArray);
            await _measurementService.SaveMeasurement(LoadedMeasurement);

            memoryStream.Dispose();

            LoadPhoto(byteArray);
        }

        private async Task<Stream> TakePictureAsync()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return null;
            }

            var result = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                CompressionQuality = 30,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
            });

            if (result == null)
                return null;

            var stream = result.GetStream();

            return stream;
        }
    }
}