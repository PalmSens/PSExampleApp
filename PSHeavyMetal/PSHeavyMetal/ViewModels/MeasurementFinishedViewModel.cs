using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    internal class MeasurementFinishedViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementFinishedViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;

            ShowPlotCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.MeasurementPlotView));
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public ICommand ShowPlotCommand { get; }
    }
}