using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class MeasurementDataViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private HeavyMetalMeasurement _loadedMeasurement;

        public MeasurementDataViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            LoadedMeasurement = _measurementService.ActiveMeasurement;

            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
        }

        public HeavyMetalMeasurement LoadedMeasurement
        {
            get => _loadedMeasurement;
            set => SetProperty(ref _loadedMeasurement, value);
        }

        public ICommand ShowPlotCommand { get; }
    }
}