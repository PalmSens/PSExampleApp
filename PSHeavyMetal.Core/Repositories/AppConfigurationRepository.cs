using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Repositories
{
    public class AppConfigurationRepository : IAppConfigurationRepository
    {
        private readonly IDataOperations _dataOperations;

        public AppConfigurationRepository(IDataOperations dataOperations)
        {
            _dataOperations = dataOperations;
        }

        public Task<List<MethodConfiguration>> LoadMethodAsync()
        {
            //The method collection should only contain one method
            return _dataOperations.GetAllAsync<MethodConfiguration>();
        }

        public Task SaveMethodAsync(MethodConfiguration method)
        {
            _dataOperations.DeleteAll<MethodConfiguration>();
            return _dataOperations.SaveAsync(method);
        }
    }
}