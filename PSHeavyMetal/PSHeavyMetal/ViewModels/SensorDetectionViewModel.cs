using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class SensorDetectionViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;

        public SensorDetectionViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }
    }
}