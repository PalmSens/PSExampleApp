using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class ConfigureMeasurementViewModel : BaseViewModel
    {
        private IMeasurementService _measurementService;

        public ConfigureMeasurementViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            ActiveMeasurement = _measurementService.ActiveMeasurement;

            SetPbConfigurationCommand = CommandFactory.Create(async () => await SetPbConfiguration());
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }
        public ICommand SetCdConfigurationCommand { get; }
        public ICommand SetCuConfigurationCommand { get; }
        public ICommand SetPbConfigurationCommand { get; }

        public async Task SetPbConfiguration()
        {
            await NavigationDispatcher.Push(NavigationViewType.SensorDetectionView);
        }
    }
}