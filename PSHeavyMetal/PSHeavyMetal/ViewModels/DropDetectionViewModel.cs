using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class DropDetectionViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;

        public DropDetectionViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;
            ContinueCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.RunMeasurementView));
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }
        public ICommand ContinueCommand { get; }
    }
}