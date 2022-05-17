using PalmSens;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;

namespace PSHeavyMetal.Core.Services
{
    public class MeasurementService : IMeasurementService
    {
        private readonly IMeasurementRepository _measurementRepository;

        public MeasurementService(IMeasurementRepository repository)
        {
            _measurementRepository = repository;
        }

        public HeavyMetalMeasurement ActiveMeasurement { get; private set; }

        public HeavyMetalMeasurement CreateMeasurement(string name, string description)
        {
            var measurement = new HeavyMetalMeasurement { Name = name, Description = description, Id = Guid.NewGuid() };
            this.ActiveMeasurement = measurement;
            return measurement;
        }

        public void CreatePbMethod()
        {
        }
    }
}