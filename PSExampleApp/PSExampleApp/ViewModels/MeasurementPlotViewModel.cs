using MvvmHelpers;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using PalmSens.Analysis;
using PalmSens.Core.Simplified.Data;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class MeasurementPlotViewModel : BaseAppViewModel
    {
        private readonly IMeasurementService _measurementService;
        private LineSeries _lineSeries;
        private SimpleCurve _simpleCurve;

        public MeasurementPlotViewModel(IMeasurementService measurementService, IAppConfigurationService appConfigurationService) : base(appConfigurationService)
        {
            _measurementService = measurementService;
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; private set; }
        public ICommand OnPageAppearingCommand { get; set; }

        public PlotModel PlotModel { get; set; } = new PlotModel();

        private void Curve_DetectedPeaks(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initializes the plot _lineSeries for the peak.
        /// </summary>
        /// <param name="simpleCurvePlotData">The lineseries of the SimpleCurve in the plot.</param>
        /// <param name="peak">The peak.</param>
        private void InitPeakPlotData(LineSeries simpleCurvePlotData, Peak peak)
        {
            //Create a new lineseries for the peak based on that of its respective SimpleCurve
            var peakLines = new LineSeries()
            {
                RenderInLegend = false,
                Color = OxyColors.Gray,
                BrokenLineStyle = LineStyle.Dash,
                MarkerType = MarkerType.None,
                YAxisKey = simpleCurvePlotData.YAxisKey
            };

            //Adds the _lineSeries for the lines used to draw the peak
            peakLines.Points.Add(new DataPoint(peak.LeftX, peak.LeftY));
            peakLines.Points.Add(new DataPoint(peak.PeakX, peak.OffsetY));
            peakLines.Points.Add(new DataPoint(peak.PeakX, peak.PeakY));
            peakLines.Points.Add(new DataPoint(peak.PeakX, peak.OffsetY));
            peakLines.Points.Add(new DataPoint(peak.RightX, peak.RightY));
            
            var verticalAlignment = VerticalAlignment.Bottom;
            if (peak.PeakY < PlotModel.PlotArea.Top + 30) // Adjust 30 as needed to ensure the label is visible
            {
                verticalAlignment = VerticalAlignment.Top;
            }

            // Create the label annotation
            var label = new PointAnnotation()
            {
                YAxisKey = simpleCurvePlotData.YAxisKey,
                X = peak.PeakX,
                Y = peak.PeakY,
                Fill = OxyColors.Blue,
                Stroke = OxyColors.Black,
                TextColor = OxyColors.Blue,
                Shape = MarkerType.Circle,
                Text = peak.PeakValue.ToString(),
                TextVerticalAlignment = verticalAlignment,
                TextHorizontalAlignment = HorizontalAlignment.Left
            };

            
            PlotModel.Annotations.Add(label);
            PlotModel.Series.Add(peakLines);
        }

        private void OnPageAppearing()
        {
            ActiveMeasurement = _measurementService.ActiveMeasurement;

            _simpleCurve = ActiveMeasurement.Measurement.SimpleCurveCollection.First();

            double[] x = _simpleCurve.XAxisValues;
            double[] y = _simpleCurve.YAxisValues;
            int pointCount = Math.Min(x.Length, y.Length);

            _lineSeries = new LineSeries()
            {
                Title = ActiveMeasurement.Configuration.AnalyteName,
            };

            //Add the SimpleCurve _lineSeries to the lineseries for the plot
            for (int i = 0; i < pointCount; i++)
                _lineSeries.Points.Add(new DataPoint(x[i], y[i]));

            var xAxis = new LinearAxis
            {
                IsAxisVisible = true,
                Position = AxisPosition.Bottom,
                Title = $"Potential/{_simpleCurve.XUnit}"
            };

            var yAxis = new LinearAxis
            {
                IsAxisVisible = true,
                Position = AxisPosition.Left,
                Title = $"Current/{_simpleCurve.YUnit}"
            };

            PlotModel.Axes.Add(xAxis);
            PlotModel.Axes.Add(yAxis);
            PlotModel.Series.Add(_lineSeries);

            if (_simpleCurve.Peaks.nPeaks > 0)
            {
                InitPeakPlotData(_lineSeries, _simpleCurve.Peaks[0]);
            }
            PlotModel.InvalidatePlot(true);
        }
    }
}