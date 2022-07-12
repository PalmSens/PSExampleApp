using PalmSens;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Repositories;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class AppConfigurationService : IAppConfigurationService
    {
        private const string DefaultMethod = "PSDiffPulse.psmethod";
        private readonly IAppConfigurationRepository _appConfigurationRepository;
        private readonly ILoadAssetsService _loadAssetsService;
        private readonly ILoadSavePlatformService _loadSavePlatformService;

        public AppConfigurationService(IAppConfigurationRepository appConfigurationRepository, ILoadAssetsService loadAssetsService, ILoadSavePlatformService loadSavePlatformService)
        {
            _appConfigurationRepository = appConfigurationRepository;
            _loadAssetsService = loadAssetsService;
            _loadSavePlatformService = loadSavePlatformService;
        }

        public async Task InitiliazeMethod()
        {
            try
            {
                var loadedMethod = await LoadConfigurationMethod();

                //If the method is null the load the default one
                if (loadedMethod == null)
                {
                    using (var filestream = _loadAssetsService.LoadFile(DefaultMethod))
                    {
                        await SaveConfigurationMethod(filestream.BaseStream);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public async Task<Method> LoadConfigurationMethod()
        {
            try
            {
                var methods = await _appConfigurationRepository.LoadMethodAsync();

                var loadedMethod = methods.FirstOrDefault();

                if (loadedMethod == null)
                    return null;

                using (var mem = new MemoryStream(loadedMethod.SerializedMethod))
                {
                    var streamReader = new StreamReader(mem);
                    return _loadSavePlatformService.LoadMethod(streamReader);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public async Task SaveConfigurationMethod(Stream stream)
        {
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                await _appConfigurationRepository.SaveMethodAsync(new MethodConfiguration { Id = Guid.NewGuid(), SerializedMethod = mem.ToArray() });
            }
        }
    }
}