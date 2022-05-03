using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;

namespace PSHeavyMetal.Core.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        public IEnumerable<Measurement> GetAllMeasurements()
        {
            throw new NotImplementedException();
        }

        public Measurement LoadMeasurement(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid SaveMeasurement(Measurement measurement)
        {
            throw new NotImplementedException();
        }
    }
}