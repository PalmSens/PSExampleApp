using MvvmHelpers;
using PalmSens;
using PalmSens.Core.Simplified.Data;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Extentions;
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
        private readonly IAppConfigurationService _appConfigurationService;
        private readonly IDeviceService _deviceService;
        private readonly IMeasurementService _measurementService;
        private readonly IMessageService _messageService;
        private SimpleCurve _activeCurve;
        private Countdown _countdown = new Countdown();

        private bool _measurementFinished = false;
        private double _progress;
        private int _progressPercentage;

        public RunMeasurementViewModel(IMeasurementService measurementService, IMessageService messageService, IDeviceService deviceService, IAppConfigurationService appConfigurationService)
        {
            Progress = 0;
            _appConfigurationService = appConfigurationService;
            _deviceService = deviceService;
            _messageService = messageService;
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

        private void _measurementService_MeasurementEnded(object sender, EventArgs e)
        {
            _activeCurve.NewDataAdded -= ActiveSimpleCurve_NewDataAdded;
            _countdown.Ticked -= OnCountdownTicked;

            RunPeakAnalysis().WithCallback();
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
            ActiveMeasurement.MeasurementDate = DateTime.Now.Date;
            await _measurementService.SaveMeasurement(ActiveMeasurement);
            await NavigationDispatcher.Push(NavigationViewType.MeasurmentFinished);
        }

        private void Curve_DetectedPeaks(object sender, EventArgs e)
        {
            _measurementService.CalculateConcentration();

            //After the concentration is calculated we allow the user to press continue
            Progress = 1;
            ProgressPercentage = 100;
            MeasurementIsFinished = true;
        }

        private async Task<Method> LoadDiffPulseMethod()
        {
            try
            {
                return await _appConfigurationService.LoadConfigurationMethod();
            }
            catch (Exception)
            {
                // When the method file cannot be found it means that it's manually removed. In this case the app needs to be reinstalled
                MainThread.BeginInvokeOnMainThread(() => _messageService.ShortAlert("Not able to load the method. Please reinstall the heavy metal app"));
                throw;
            }
        }

        private void OnCountdownTicked()
        {
            Progress = _countdown.ElapsedTime / _countdown.TotalTimeInMilliSeconds;
            ProgressPercentage = (int)(Progress * 100);
        }

        private async Task OnPageAppearing()
        {
            var method = await LoadDiffPulseMethod();

            try
            {
                _countdown.Start((int)Math.Round(method.MinimumEstimatedMeasurementDuration * 1000));
                _countdown.Ticked += OnCountdownTicked;

                _measurementService.ActiveMeasurement.Measurement = await _measurementService.StartMeasurement(method);
            }
            catch (NullReferenceException)
            {
                // Nullreference is thrown when device is not connected anymore. In this case we pop back to homescreen. The user can then try to reconnect again
                _messageService.ShortAlert("Not connected to a device, please try reconnecting to a device again");
                this._measurementService.ResetMeasurement();
                await _deviceService.DisconnectDevice();
                await NavigationDispatcher.PopToRoot();
            }
            catch (ArgumentException)
            {
                // Argument exception is thrown when method is incompatible with the connected device.
                _messageService.ShortAlert("Device incompatible. Please select a different device");
                this._measurementService.ResetMeasurement();
                await _deviceService.DisconnectDevice();
                await NavigationDispatcher.PopToRoot();
            }
            catch (Exception ex)
            {
                _messageService.LongAlert("Something went wrong with starting a measurement please restart the device and try again");
                Debug.WriteLine(ex);
                this._measurementService.ResetMeasurement();
                await _deviceService.DisconnectDevice();
                await NavigationDispatcher.PopToRoot();
            }
        }

        private async Task RunPeakAnalysis()
        {
            _activeCurve.DetectedPeaks += Curve_DetectedPeaks;
            await _activeCurve.DetectPeaksAsync();
        }
    }
}