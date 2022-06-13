using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class DropDetectionViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;

        public DropDetectionViewModel(IMeasurementService measurementService)
        {
            _popupNavigation = PopupNavigation.Instance;
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;
            ContinueCommand = CommandFactory.Create(Continue);
            CancelCommand = CommandFactory.Create(Cancel);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public ICommand CancelCommand { get; }

        public ICommand ContinueCommand { get; }

        private async Task Cancel()
        {
            await _popupNavigation.PopAllAsync();
        }

        private async Task Continue()
        {
            await _popupNavigation.PopAllAsync();
            await NavigationDispatcher.Push(NavigationViewType.RunMeasurementView);
        }
    }
}