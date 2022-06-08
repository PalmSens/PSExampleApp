using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IMeasurementRepository
    {
        /// <summary>
        /// Loads all measurement configurations
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MeasurementConfiguration>> LoadAllConfigurations();

        /// <summary>
        /// Loads a measurement based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SavedMeasurement> LoadMeasurement(Guid id);

        /// <summary>
        /// Saves a measurement
        /// </summary>
        /// <param name="Measurement"></param>
        /// <returns></returns>
        Task SaveMeasurement(SavedMeasurement measurement);

        /// <summary>
        /// Saves
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task SaveMeasurementConfiguration(MeasurementConfiguration configuration);
    }
}