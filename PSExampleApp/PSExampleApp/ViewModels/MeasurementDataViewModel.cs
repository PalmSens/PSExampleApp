using FFImageLoading;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resx;
using PSExampleApp.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSExampleApp.Forms.ViewModels
{
    public class MeasurementDataViewModel : BaseAppViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IShareService _shareService;
        private bool _isCreatingReport;

        //private bool _hasMax
        private HeavyMetalMeasurement _loadedMeasurement;

        public MeasurementDataViewModel(IMeasurementService measurementService, IShareService shareService, IAppConfigurationService appConfigurationService, IMessageService messageService) : base(appConfigurationService)
        {
            _measurementService = measurementService;
            _messageService = messageService;
            _shareService = shareService;
            LoadedMeasurement = _measurementService.ActiveMeasurement;
            _popupNavigation = PopupNavigation.Instance;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);

            ShareMeasurementCommand = CommandFactory.Create(ShareMeasurement);
            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
            OnPhotoSelectedCommand = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
            BackCommand = CommandFactory.Create(Back);
        }

        public ICommand BackCommand { get; }

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

        private async Task Back()
        {
            await NavigationDispatcher.PopToRoot();
            _measurementService.ResetMeasurement();
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
                            LoadedMeasurement.MeasurementImages.Add(imageByteArray);
                            await _measurementService.SaveMeasurement(LoadedMeasurement);
                            LoadPhoto(imageByteArray);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _messageService.ShortAlert(AppResources.Alert_CameraPermission);
            }
        }

        private async Task<Stream> TakePictureAsync()
        {
            if (!MediaPicker.IsCaptureSupported)
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