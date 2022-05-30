using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;

namespace PSHeavyMetal.Forms.ViewModels
{
    internal class MeasurementFinishedViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;

        public MeasurementFinishedViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }
    }
}