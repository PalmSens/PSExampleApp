using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.DataAccess;
using System;
using System.Collections.Generic;
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

        public async Task DeleteMeasurement(Guid id)
        {
            await _dataOperations.DeleteByIdAsync<SavedMeasurement>(id);
        }

        public async Task DeleteMeasurementConfiguration(Guid id)
        {
            await _dataOperations.DeleteByIdAsync<MeasurementConfiguration>(id);
        }

        public async Task<IEnumerable<MeasurementConfiguration>> LoadAllConfigurations()
        {
            return await _dataOperations.GetAllAsync<MeasurementConfiguration>();
        }

        public async Task<SavedMeasurement> LoadMeasurement(Guid id)
        {
            return await _dataOperations.LoadByIdAsync<SavedMeasurement>(id);
        }

        public async Task SaveMeasurement(SavedMeasurement measurement)
        {
            await _dataOperations.SaveAsync(measurement);
        }

        public async Task SaveMeasurementConfiguration(MeasurementConfiguration configuration)
        {
            await _dataOperations.SaveAsync(configuration);
        }
    }
}