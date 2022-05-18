using PalmSens;
using PalmSens.Core.Simplified.Data;
using PalmSens.Techniques;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class RunMeasurementViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private SimpleCurve _activeCurve;

        /// <summary>
        /// The instance of method class containing the Linear Sweep Voltammetry parameters
        /// </summary>
        private LinearSweep _methodLSV;

        public RunMeasurementViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            _measurementService.DataReceived += _measurementService_DataReceived;

            ActiveMeasurement = _measurementService.ActiveMeasurement;

            ReceivedData.Add($"Data received potential 1, current 2");
            ReceivedData.Add($"Data received potential 1, current 2");

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException: ex =>
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                //DisplayAlert();
                                Console.WriteLine(ex.Message);
                            }), allowsMultipleExecutions: false);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }
        public ICommand OnPageAppearingCommand { get; }
        public ObservableCollection<string> ReceivedData { get; set; } = new ObservableCollection<string>();

        private void _measurementService_DataReceived(object sender, SimpleCurve activeSimpleCurve)
        {
            _activeCurve = activeSimpleCurve;
            activeSimpleCurve.NewDataAdded += ActiveSimpleCurve_NewDataAdded;
        }

        private void ActiveSimpleCurve_NewDataAdded(object sender, PalmSens.Data.ArrayDataAddedEventArgs e)
        {
            int startIndex = e.StartIndex; //The index of the first new data point added to the curve
            int count = e.Count; //The number of new data points added to the curve

            for (int i = startIndex; i < startIndex + count; i++)
            {
                double xValue = _activeCurve.XAxisValue(i); //Get the value on Curve's X-Axis (potential) at the specified index
                double yValue = _activeCurve.YAxisValue(i); //Get the value on Curve's Y-Axis (current) at the specified index
                ReceivedData.Add($"Data received potential {xValue}, current {yValue}");
            }
        }

        /// <summary>
        /// Initializes the LSV method.
        /// </summary>
        private void InitLSVMethod()
        {
            _methodLSV = new LinearSweep(); //Create a new linear sweep method with the default settings
            _methodLSV.BeginPotential = -.5f; //Sets the potential to start the sweep from
            _methodLSV.EndPotential = .5f - 0.05f; //Sets the potential for the sweep to stop at
            _methodLSV.StepPotential = 0.05f; //Sets the step size
            _methodLSV.Scanrate = 0.1f; //Sets the scan rate to 0.05 V/s

            _methodLSV.EquilibrationTime = 1f; //Equilabrates the cell at the defined potential for 1 second before starting the measurement
            _methodLSV.Ranging.StartCurrentRange = new CurrentRange(CurrentRanges.cr1uA); //Starts equilabration in the 1µA current range
            _methodLSV.Ranging.MinimumCurrentRange = new CurrentRange(CurrentRanges.cr10nA); //Min current range 10nA
            _methodLSV.Ranging.MaximumCurrentRange = new CurrentRange(CurrentRanges.cr1mA); //Max current range 1mA
        }

        private async Task OnPageAppearing()
        {
            InitLSVMethod();
            await _measurementService.StartMeasurement(_methodLSV);
        }
    }
}