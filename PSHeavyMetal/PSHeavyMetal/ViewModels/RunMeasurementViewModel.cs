using MvvmHelpers;
using PalmSens.Core.Simplified.Data;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private Countdown _countdown = new Countdown();

        private bool _measurementFinished = false;
        private double _progress;
        private int _progressPercentage;

        public RunMeasurementViewModel(IMeasurementService measurementService)
        {
            Progress = 0;
            _measurementService = measurementService;
            _measurementService.DataReceived += _measurementService_DataReceived;
            _measurementService.MeasurementEnded += _measurementService_MeasurementEnded;

            ActiveMeasurement = _measurementService.ActiveMeasurement;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing, onException: ex =>
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                //DisplayAlert();
                                Console.WriteLine(ex.Message);
                            }), allowsMultipleExecutions: false);

            ContinueCommand = CommandFactory.Create(Continue);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public ICommand ContinueCommand { get; }

        public bool MeasurementIsFinished
        {
            get => _measurementFinished;
            set => SetProperty(ref _measurementFinished, value);
        }

        public ICommand OnPageAppearingCommand { get; }

        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public int ProgressPercentage
        {
            get => _progressPercentage;
            set => SetProperty(ref _progressPercentage, value);
        }

        public ObservableCollection<string> ReceivedData { get; set; } = new ObservableCollection<string>();

        private void _measurementService_DataReceived(object sender, SimpleCurve activeSimpleCurve)
        {
            _activeCurve = activeSimpleCurve;
            activeSimpleCurve.NewDataAdded += ActiveSimpleCurve_NewDataAdded;
        }

        private async void _measurementService_MeasurementEnded(object sender, EventArgs e)
        {
            _activeCurve.NewDataAdded -= ActiveSimpleCurve_NewDataAdded;
            _countdown.Ticked -= OnCountdownTicked;

            await RunPeakAnalysis();
        }

        private void ActiveSimpleCurve_NewDataAdded(object sender, PalmSens.Data.ArrayDataAddedEventArgs e)
        {
            int startIndex = e.StartIndex; //The index of the first new data point added to the curve
            int count = e.Count; //The number of new data points added to the curve

            for (int i = startIndex; i < startIndex + count; i++)
            {
                double xValue = _activeCurve.XAxisValue(i); //Get the value on Curve's X-Axis (potential) at the specified index
                double yValue = _activeCurve.YAxisValue(i); //Get the value on Curve's Y-Axis (current) at the specified index

                Debug.WriteLine($"Data received potential {xValue}, current {yValue}");
                ReceivedData.Add($"potential {xValue}, current {yValue}");
            }
        }

        private async Task Continue()
        {
            //The continue will trigger the save of the measurement. //TODO maybe add cancel in case user doesn't want to save
            await _measurementService.SaveMeasurement(ActiveMeasurement);

            var blah = await _measurementService.LoadMeasurement(ActiveMeasurement.Id);

            await NavigationDispatcher.Push(NavigationViewType.MeasurmentFinished);
        }

        private async void Curve_DetectedPeaks(object sender, EventArgs e)
        {
            _measurementService.CalculateConcentration();

            //After the concentration is calculated we allow the user to press continue
            Progress = 1;
            ProgressPercentage = 100;
            MeasurementIsFinished = true;
        }

        private void OnCountdownTicked()
        {
            Progress = _countdown.ElapsedTime / _countdown.TotalTimeInMilliSeconds;
            ProgressPercentage = (int)(Progress * 100);
        }

        private async Task OnPageAppearing()
        {
            var method = _measurementService.LoadMethod("PSDiffPulse.psmethod");

            _countdown.Start((int)Math.Round(method.MinimumEstimatedMeasurementDuration * 1000));
            _countdown.Ticked += OnCountdownTicked;

            _measurementService.ActiveMeasurement.Measurement = await _measurementService.StartMeasurement(method);
        }

        private async Task RunPeakAnalysis()
        {
            _activeCurve.DetectedPeaks += Curve_DetectedPeaks;
            await _activeCurve.DetectPeaksAsync(0.05, 0.005);
        }
    }
}