using PalmSens;
using PSHeavyMetal.Common.Models;
using System.IO;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IAppConfigurationService
    {
        public ApplicationSettings CurrentApplicationSettings { get; }

        public StreamReader GetBackgroundImage();

        public ApplicationSettings GetSettings();

        public Task<ApplicationSettings> GetSettingsAsync();

        /// <summary>
        /// Initializes the method configuration. It loads the current method configuration. If there aren't any methods then it will loads the default method
        /// </summary>
        /// <returns></returns>
        public Task InitializeMethod();

        public Task<Method> LoadConfigurationMethod();

        public Task SaveConfigurationMethod(Stream stream);

        /// <summary>
        /// A non async version to save the default settings during startup
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public void SaveSettings(ApplicationSettings settings);

        public Task SaveSettingsAsync(ApplicationSettings settings);

        public Task SaveTitle(string title);
    }
}