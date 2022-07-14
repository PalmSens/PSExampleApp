using PSExampleApp.Common.Models;
using PSExampleApp.Core.DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSExampleApp.Core.Repositories
{
    public class AppConfigurationRepository : IAppConfigurationRepository
    {
        private readonly IDataOperations _dataOperations;

        public AppConfigurationRepository(IDataOperations dataOperations)
        {
            _dataOperations = dataOperations;
        }

        public List<ApplicationSettings> LoadApplicationSettings()
        {
            return _dataOperations.GetAll<ApplicationSettings>();
        }

        public Task<List<ApplicationSettings>> LoadApplicationSettingsAsync()
        {
            return _dataOperations.GetAllAsync<ApplicationSettings>();
        }

        public Task<List<MethodConfiguration>> LoadMethodAsync()
        {
            //The method collection should only contain one method
            return _dataOperations.GetAllAsync<MethodConfiguration>();
        }

        public void SaveApplicationSettings(ApplicationSettings applicationSettings)
        {
            _dataOperations.Save(applicationSettings);
        }

        public Task SaveApplicationSettingsAsync(ApplicationSettings applicationSettings)
        {
            return _dataOperations.SaveAsync(applicationSettings);
        }

        public Task SaveMethodAsync(MethodConfiguration method)
        {
            _dataOperations.DeleteAll<MethodConfiguration>();
            return _dataOperations.SaveAsync(method);
        }
    }
}