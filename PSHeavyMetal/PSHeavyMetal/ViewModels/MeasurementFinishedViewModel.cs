using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.ViewModels
{
    internal class MeasurementFinishedViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IShareService _shareService;
        private bool _IsCreatingReport;

        public MeasurementFinishedViewModel(IMeasurementService measurementService, IShareService shareService)
        {
            _shareService = shareService;
            _popupNavigation = PopupNavigation.Instance;
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;

            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
            NavigateToHomeCommand = CommandFactory.Create(NavigateToHome);
            RepeatMeasurementCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.ConfigureMeasurementView));
            OnPhotoSelected = CommandFactory.Create(async photo => await OpenPhoto(photo as ImageSource));
            TakePhotoCommand = CommandFactory.Create(TakePhoto);
            ShareMeasurementCommand = CommandFactory.Create(ShareMeasurement);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public bool IsCreatingReport
        {
            get => _IsCreatingReport;
            set => SetProperty(ref _IsCreatingReport, value);
        }

        public ObservableCollection<ImageSource> MeasurementPhotos { get; } = new ObservableCollection<ImageSource>();

        public ICommand NavigateToHomeCommand { get; }

        public ICommand OnPhotoSelected { get; }
        public ICommand RepeatMeasurementCommand { get; }
        public ICommand ShareMeasurementCommand { get; }
        public ICommand ShowPlotCommand { get; }
        public ICommand TakePhotoCommand { get; }

        public async Task NavigateToHome()
        {
            _measurementService.ResetMeasurement();
            await NavigationDispatcher.PopToRoot();
        }

        private async Task OpenPhoto(ImageSource imageSource)
        {
            await _popupNavigation.PushAsync(new MeasurementPhotoPopup(imageSource));
        }

        private async Task ShareMeasurement()
        {
            var cacheFile = Path.Combine(FileSystem.CacheDirectory, $"Report{ActiveMeasurement.Name}");
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