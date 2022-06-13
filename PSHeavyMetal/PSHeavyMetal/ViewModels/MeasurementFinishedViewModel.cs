using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Threading.Tasks;
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
            NavigateToHomeCommand = CommandFactory.Create(NavigateToHome);
            RepeatMeasurementCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.ConfigureMeasurementView));
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public ICommand NavigateToHomeCommand { get; }

        public ICommand RepeatMeasurementCommand { get; }

        public ICommand ShowPlotCommand { get; }

        public async Task NavigateToHome()
        {
            _measurementService.ResetMeasurement();
            await NavigationDispatcher.PopToRoot();
        }
    }
}