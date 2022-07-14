using PSExampleApp.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PSExampleApp.Core.Repositories
{
    public interface IAppConfigurationRepository
    {
        /// <summary>
        /// Loads the application settings
        /// </summary>
        /// <returns></returns>
        List<ApplicationSettings> LoadApplicationSettings();

        /// <summary>
        /// Loads the application settings
        /// </summary>
        /// <returns></returns>
        Task<List<ApplicationSettings>> LoadApplicationSettingsAsync();

        /// <summary>
        /// Loads the method saved in the database. This can be only one method
        /// </summary>
        /// <returns></returns>
        Task<List<MethodConfiguration>> LoadMethodAsync();

        /// <summary>
        /// Saves the application application settings. This is the non-sync version used at the startup
        /// </summary>
        /// <param name="applicationSettings"></param>
        void SaveApplicationSettings(ApplicationSettings applicationSettings);

        /// <summary>
        /// Saves the applicatoin settings
        /// </summary>
        /// <param name="applicationSettings"></param>
        /// <returns></returns>
        Task SaveApplicationSettingsAsync(ApplicationSettings applicationSettings);

        /// <summary>
        /// Saves a method to the database. It first deletes the
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        Task SaveMethodAsync(MethodConfiguration method);
    }
}