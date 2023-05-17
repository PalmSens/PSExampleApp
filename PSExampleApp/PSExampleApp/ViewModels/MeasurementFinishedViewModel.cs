using FFImageLoading;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resx;
using PSExampleApp.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSExampleApp.Forms.ViewModels
{
    internal class MeasurementFinishedViewModel : BaseAppViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IShareService _shareService;
        private bool _IsCreatingReport;
        private readonly IMessageService _messageService;

        public MeasurementFinishedViewModel(IMeasurementService measurementService, IShareService shareService, IAppConfigurationService appConfigurationService, IMessageService messageService) : base(appConfigurationService)
        {
            _shareService = shareService;
            _popupNavigation = PopupNavigation.Instance;
            _measurementService = measurementService;
            _messageService = messageService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;

            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
            NavigateToHomeCommand = CommandFactory.Create(NavigateToHome);
            RepeatMeasurementCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.SelectAnalyteView));
            OnPhotoSelectedCommand = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
            ShareMeasurementCommand = CommandFactory.Create(ShareMeasurement);
            OnPageAppearingCommand = CommandFactory.Create(OnAppearing);
            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        /// <summary>
        /// Gets if the measurement has a maximum amount of photos. This is 3
        /// </summary>
        public bool HasMaxPhotos => MeasurementPhotos.Count == 3;

        public bool IsCreatingReport
        {
            get => _IsCreatingReport;
            set => SetProperty(ref _IsCreatingReport, value);
        }

        public ObservableCollection<MeasurementPhotoPresenter> MeasurementPhotos { get; } = new ObservableCollection<MeasurementPhotoPresenter>();

        public ICommand NavigateToHomeCommand { get; }

        public ICommand OnPageAppearingCommand { get; }

        public ICommand OnPageDisappearingCommand { get; }

        public ICommand OnPhotoSelectedCommand { get; }

        public ICommand RepeatMeasurementCommand { get; }

        public ICommand ShareMeasurementCommand { get; }

        public ICommand ShowPlotCommand { get; }

        public ICommand TakePhotoCommand { get; }

        public async Task NavigateToHome()
        {
            _measurementService.ResetMeasurement();
            await NavigationDispatcher.PopToRoot();
        }

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

        private void OnAppearing()
        {
            MeasurementPhotos.CollectionChanged += MeasurementPhotos_CollectionChanged;

            this.OnPropertyChanged(nameof(HasMaxPhotos));
        }

        private void OnDisappearing()
        {
            MeasurementPhotos.CollectionChanged -= MeasurementPhotos_CollectionChanged;
        }

        private async Task OpenPhoto(ImageSource imageSource)
        {
            await _popupNavigation.PushAsync(new MeasurementPhotoPopup(imageSource));
        }

        private async Task ShareMeasurement()
        {
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, $"Report-{ActiveMeasurement.Name}.pdf");
            IsCreatingReport = true;

            //CreatePDFfile is a long running proces that isn't async by itself.
            await Task.Run(() => _shareService.CreatePdfFile(ActiveMeasurement, cacheFile));

            IsCreatingReport = false;

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Sharing is Caring",
                File = new ShareFile(cacheFile)
            });
        }

        private async Task TakePhoto()
        {
            MeasurementPhotos.CollectionChanged += MeasurementPhotos_CollectionChanged;

            try
            {
                using (var fullImageStream = await TakePictureAsync())
                {
                    using (var reducedImageStream = await ImageService.Instance.LoadStream(token => Task.FromResult(fullImageStream)).DownSample(300).AsJPGStreamAsync())
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await reducedImageStream.CopyToAsync(memoryStream);
                            var imageByteArray = memoryStream.ToArray();
                            ActiveMeasurement.MeasurementImages.Add(imageByteArray);
                            await _measurementService.SaveMeasurement(ActiveMeasurement);
                            LoadPhoto(imageByteArray);
                        }
                    }
                }
            }
            catch (System.Exception)
            {

                _messageService.ShortAlert(AppResources.Alert_CameraPermission);
            }

        }

        private async Task<Stream> TakePictureAsync()
        {

            if(!MediaPicker.IsCaptureSupported)
            {
                return null;
            }


            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
            {
                return null;
            }
            return await photo.OpenReadAsync();

        }
    }
}