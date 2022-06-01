using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.DataAccess;
using System;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public class MeasurementRepository : IMeasurementRepository
    {
        private readonly IDataOperations _dataOperations;

        public MeasurementRepository(IDataOperations dataOperations)
        {
            _dataOperations = dataOperations;
        }

        public async Task<SavedMeasurement> LoadMeasurement(Guid id)
        {
            return await _dataOperations.LoadByIdAsync<SavedMeasurement>(id);
        }

        public async Task SaveMeasurement(SavedMeasurement measurement)
        {
            await _dataOperations.SaveAsync(measurement);
        }
    }
}