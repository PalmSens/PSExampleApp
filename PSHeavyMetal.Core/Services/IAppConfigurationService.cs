using PalmSens;
using System.IO;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IAppConfigurationService
    {
        /// <summary>
        /// Initializes the method configuration. It loads the current method configuration. If there aren't any methods then it will loads the default method
        /// </summary>
        /// <returns></returns>
        public Task InitiliazeMethod();

        public Task<Method> LoadConfigurationMethod();

        public Task SaveConfigurationMethod(Stream stream);
    }
}