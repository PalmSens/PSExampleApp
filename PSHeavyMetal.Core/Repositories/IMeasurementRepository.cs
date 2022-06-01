using PSHeavyMetal.Common.Models;
using System;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IMeasurementRepository
    {
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
    }
}