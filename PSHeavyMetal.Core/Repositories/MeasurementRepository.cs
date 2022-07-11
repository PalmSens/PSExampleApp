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

        public Task DeleteMeasurement(Guid id)
        {
            return _dataOperations.DeleteByIdAsync<SavedMeasurement>(id);
        }

        public Task DeleteMeasurementConfiguration(Guid id)
        {
            return _dataOperations.DeleteByIdAsync<MeasurementConfiguration>(id);
        }

        public Task<List<MeasurementConfiguration>> LoadAllConfigurations()
        {
            return _dataOperations.GetAllAsync<MeasurementConfiguration>();
        }

        public Task<SavedMeasurement> LoadMeasurement(Guid id)
        {
            return _dataOperations.LoadByIdAsync<SavedMeasurement>(id);
        }

        public Task SaveMeasurement(SavedMeasurement measurement)
        {
            return _dataOperations.SaveAsync(measurement);
        }

        public Task SaveMeasurementConfiguration(MeasurementConfiguration configuration)
        {
            return _dataOperations.SaveAsync(configuration);
        }
    }
}