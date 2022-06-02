using PalmSens;
using PalmSens.Analysis;
using PalmSens.Core.Simplified.Data;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private readonly IUserService _userService;

        public MeasurementService(
            IMeasurementRepository repository,
            InstrumentService instrumentService,
            ILoadSavePlatformService loadSavePlatformService,
            ILoadAssetsService loadAssetsService,
            IUserService userService)
        {
            _measurementRepository = repository;
            _instrumentService = instrumentService;
            _loadSavePlatformService = loadSavePlatformService;
            _loadAssetsService = loadAssetsService;
            _userService = userService;
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
            if (ActiveMeasurement.Measurement.SimpleCurveCollection.Count > 1)
                throw new InvalidOperationException("The measurement has multiple curves");

            var activeCurve = ActiveMeasurement.Measurement.SimpleCurveCollection[0];

            for (int i = 0; i < activeCurve.Peaks.nPeaks; i++)
            {
                //We first put the peaks in a list. Easier to filter this way
                peakList.Add(activeCurve.Peaks[i]);
            }

            //Here we filter based on the expected peak and the peakwidth. If the PeakX value falls within the width then the peak fill be used.
            //If we find multiple peaks then we select the one one with the highest peak value
            var peakWidthLeft = ActiveMeasurement.Configuration.ConcentrationMethod.ExpectedPeakOnXAxis - ActiveMeasurement.Configuration.ConcentrationMethod.PeakWidth;
            var peakWidthRight = ActiveMeasurement.Configuration.ConcentrationMethod.ExpectedPeakOnXAxis + ActiveMeasurement.Configuration.ConcentrationMethod.PeakWidth;
            var filteredPeak = peakList.Where(x => x.PeakX >= peakWidthLeft && x.PeakX <= peakWidthRight).OrderByDescending(y => y.PeakValue).FirstOrDefault();

            //If we can't find any filtered peaks then we leave the concentration at 0
            if (filteredPeak == null)
                return;

            ActiveMeasurement.Configuration.Concentration = Math.Truncate(ActiveMeasurement.Configuration.ConcentrationMethod.Slope * filteredPeak.PeakValue + ActiveMeasurement.Configuration.ConcentrationMethod.Constant);
        }

        public HeavyMetalMeasurement CreateMeasurement(string name, string description)
        {
            var measurement = new HeavyMetalMeasurement
            {
                Name = name,
                Id = Guid.NewGuid(),
                Configuration = new MeasurementConfiguration { Description = description ?? "" },
            };

            this.ActiveMeasurement = measurement;
            return measurement;
        }

        public async Task<HeavyMetalMeasurement> LoadMeasurement(Guid id)
        {
            var savedMeasurement = await _measurementRepository.LoadMeasurement(id);

            var heavyMetalMeasurement = new HeavyMetalMeasurement
            {
                Configuration = savedMeasurement.Configuration,
                Id = id,
                Name = savedMeasurement.Name,
            };

            using (var stream = new MemoryStream(savedMeasurement.SerializedMeasurement))
            {
                heavyMetalMeasurement.Measurement = _loadSavePlatformService.LoadMeasurement(stream);
            }

            return heavyMetalMeasurement;
        }

        public Method LoadMethod(string filename)
        {
            using (var filestream = _loadAssetsService.LoadFile(filename))
            {
                return _loadSavePlatformService.LoadMethod(filestream);
            }
        }

        public async Task SaveMeasurement(HeavyMetalMeasurement measurement)
        {
            byte[] array;

            using (var memoryStream = new MemoryStream())
            {
                await _loadSavePlatformService.SaveMeasurementToStreamAsync(measurement.Measurement, memoryStream);
                array = memoryStream.ToArray();
            }

            var savedMeasurement = new SavedMeasurement
            {
                SerializedMeasurement = array,
                Configuration = measurement.Configuration,
                Id = measurement.Id,
                Name = measurement.Name,
            };

            try
            {
                await _measurementRepository.SaveMeasurement(savedMeasurement);
                await _userService.SaveMeasurementInfo(measurement);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Saving of the measurment failed {ex}");
                throw;
            }
        }

        public void SetCalculationMethod(MethodType method)
        {
            switch (method)
            {
                case MethodType.Pb:
                    this.ActiveMeasurement.Configuration.ConcentrationMethod = new ConcentrationMethod
                    {
                        Constant = 50,
                        Slope = 1000,
                        ExpectedPeakOnXAxis = -0.02,
                        PeakWidth = 0.2
                    };
                    this.ActiveMeasurement.Configuration.MethodType = method;
                    this.ActiveMeasurement.Configuration.ConcentrationUnit = ConcentrationUnit.ppm;
                    this.ActiveMeasurement.Configuration.AnalyteName = "Lead";
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