using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IMeasurementRepository
    {
        /// <summary>
        /// Loads a measurement based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Measurement LoadMeasurement(Guid id);

        /// <summary>
        /// Saves a measurement
        /// </summary>
        /// <param name="Measurementname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Guid SaveMeasurement(Measurement measurement);

        /// <summary>
        /// Get all saved measurements
        /// </summary>
        /// <returns></returns>
        IEnumerable<Measurement> GetAllMeasurements();
    }
}