using MvvmHelpers;
using OxyPlot;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class MeasurementPlotViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementPlotViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; private set; }
        public ICommand OnPageAppearingCommand { get; set; }

        public PlotModel PlotModel { get; set; }

        private void OnPageAppearing()
        {
            ActiveMeasurement = _measurementService.ActiveMeasurement;
        }
    }
}