using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;

namespace PSHeavyMetal.Core.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        public IEnumerable<HeavyMetalMeasurement> GetAllMeasurements()
        {
            throw new NotImplementedException();
        }

        public HeavyMetalMeasurement LoadMeasurement(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid SaveMeasurement(HeavyMetalMeasurement measurement)
        {
            throw new NotImplementedException();
        }
    }
}