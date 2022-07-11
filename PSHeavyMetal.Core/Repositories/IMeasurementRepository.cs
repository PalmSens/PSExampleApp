using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IMeasurementRepository
    {
        /// <summary>
        /// Deletes the measurement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteMeasurement(Guid id);

        /// <summary>
        /// Deletes the configuration of the measurement
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteMeasurementConfiguration(Guid id);

        /// <summary>
        /// Loads all measurement configurations
        /// </summary>
        /// <returns></returns>
        Task<List<MeasurementConfiguration>> LoadAllConfigurations();

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