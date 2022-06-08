using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Collections.ObjectModel;
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
            OnConfigSelected = CommandFactory.Create(async conf => await SetConfiguration(conf as MeasurementConfiguration));
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
        }

        public ObservableCollection<MeasurementConfiguration> MeasurementConfigurations { get; } = new ObservableCollection<MeasurementConfiguration>();
        public ICommand OnConfigSelected { get; }
        public ICommand OnPageAppearingCommand { get; }

        private async Task OnPageAppearing()
        {
            var configurations = await _measurementService.LoadAllMeasurementConfigurationsAsync();

            foreach (var config in configurations)
                MeasurementConfigurations.Add(config);
        }

        private async Task SetConfiguration(MeasurementConfiguration configuration)
        {
            _measurementService.CreateMeasurement(configuration);

            await NavigationDispatcher.Push(NavigationViewType.PrepareMeasurementView);
        }
    }
}