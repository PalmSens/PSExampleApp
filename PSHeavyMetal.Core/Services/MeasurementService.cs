using PalmSens;
using PalmSens.Core.Simplified.Data;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;
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

        public async Task<SimpleMeasurement> StartMeasurement(Method method)
        {
            return await _instrumentService.MeasureAsync(method);
        }
    }
}