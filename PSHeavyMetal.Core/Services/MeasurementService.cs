using PalmSens;
using PalmSens.Analysis;
using PalmSens.Core.Simplified.Data;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PalmSens.Core.Simplified.PSCommSimple;

namespace PSHeavyMetal.Core.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly InstrumentService _instrumentService;
        private readonly ILoadAssetsService _loadAssetsService;
        private readonly ILoadSavePlatformService _loadSavePlatformService;
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementService(IMeasurementRepository repository, InstrumentService instrumentService, ILoadSavePlatformService loadSavePlatformService, ILoadAssetsService loadAssetsService)
        {
            _measurementRepository = repository;
            _instrumentService = instrumentService;
            _loadSavePlatformService = loadSavePlatformService;
            _loadAssetsService = loadAssetsService;
        }

        public event SimpleCurveStartReceivingDataHandler DataReceived
        {
            add => _instrumentService.SimpleCurveStartReceivingData += value;
            remove => _instrumentService.SimpleCurveStartReceivingData -= value;
        }

        public event EventHandler MeasurementEnded
        {
            add => _instrumentService.MeasurementEnded += value;
            remove => _instrumentService.MeasurementEnded -= value;
        }

        public event EventHandler MeasurementStarted
        {
            add => _instrumentService.MeasurementStarted += value;
            remove => _instrumentService.MeasurementStarted -= value;
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; private set; }

        public void CalculateConcentration()
        {
            var peakList = new List<Peak>();

            //We assume that the result of the measurement will be in 1 curve
            if (ActiveMeasurement.ConfiguredMeasurement.SimpleCurveCollection.Count > 1)
                throw new InvalidOperationException("The measurement has multiple curves");

            var activeCurve = ActiveMeasurement.ConfiguredMeasurement.SimpleCurveCollection[0];

            for (int i = 0; i < activeCurve.Peaks.nPeaks; i++)
            {
                //We first put the peaks in a list. Easier to filter this way
                peakList.Add(activeCurve.Peaks[i]);
            }

            //Here we filter based on the expected peak and the peakwidth. If the PeakX value falls within the width then the peak fill be used.
            //If we find multiple peaks then we select the one one with the highest peak value
            var peakWidthLeft = ActiveMeasurement.ConcentrationMethod.ExpectedPeakOnXAxis - ActiveMeasurement.ConcentrationMethod.PeakWidth;
            var peakWidthRight = ActiveMeasurement.ConcentrationMethod.ExpectedPeakOnXAxis + ActiveMeasurement.ConcentrationMethod.PeakWidth;
            var filteredPeak = peakList.Where(x => x.PeakX >= peakWidthLeft && x.PeakX <= peakWidthRight).OrderByDescending(y => y.PeakValue).FirstOrDefault();

            //If we can't find any filtered peaks then we leave the concentration at 0
            if (filteredPeak == null)
                return;

            ActiveMeasurement.Concentration = Math.Truncate(ActiveMeasurement.ConcentrationMethod.Slope * filteredPeak.PeakValue + ActiveMeasurement.ConcentrationMethod.Constant);
        }

        public HeavyMetalMeasurement CreateMeasurement(string name, string description)
        {
            var measurement = new HeavyMetalMeasurement { Name = name, Description = description, Id = Guid.NewGuid() };
            this.ActiveMeasurement = measurement;
            return measurement;
        }

        public Method LoadMethod(string filename)
        {
            using (var filestream = _loadAssetsService.LoadFile(filename))
            {
                return _loadSavePlatformService.LoadMethod(filestream);
            }
        }

        public void SetCalculationMethod(MethodType method)
        {
            switch (method)
            {
                case MethodType.Pb:
                    this.ActiveMeasurement.ConcentrationMethod = new ConcentrationMethod
                    {
                        Constant = 50,
                        Slope = 1000,
                        ExpectedPeakOnXAxis = -0.02,
                        PeakWidth = 0.2
                    };
                    this.ActiveMeasurement.MethodType = method;
                    this.ActiveMeasurement.ConcentrationUnit = ConcentrationUnit.ppm;
                    this.ActiveMeasurement.AnalyteName = "Lead";
                    break;

                default:
                    throw new NotImplementedException("Method not implemented");
            }
        }

        public async Task<SimpleMeasurement> StartMeasurement(Method method)
        {
            return await _instrumentService.MeasureAsync(method);
        }
    }
}