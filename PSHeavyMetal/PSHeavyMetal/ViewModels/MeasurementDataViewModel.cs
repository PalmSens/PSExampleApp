using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;

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
        }

        public HeavyMetalMeasurement LoadedMeasurement
        {
            get => _loadedMeasurement;
            set => SetProperty(ref _loadedMeasurement, value);
        }
    }
}