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
        private const string DefaultBackground = "background.jpeg";
        private const string DefaultMethod = "PSDiffPulse.psmethod";
        private readonly IAppConfigurationRepository _appConfigurationRepository;
        private readonly ILoadAssetsService _loadAssetsService;
        private readonly ILoadSavePlatformService _loadSavePlatformService;
        private ApplicationSettings _applicationSettings;

        public AppConfigurationService(IAppConfigurationRepository appConfigurationRepository, ILoadAssetsService loadAssetsService, ILoadSavePlatformService loadSavePlatformService)
        {
            _appConfigurationRepository = appConfigurationRepository;
            _loadAssetsService = loadAssetsService;
            _loadSavePlatformService = loadSavePlatformService;
        }

        public ApplicationSettings CurrentApplicationSettings
        {
            get => _applicationSettings;
            set => _applicationSettings = value;
        }

        public StreamReader GetBackgroundImage()
        {
            return _loadAssetsService.LoadFile(DefaultBackground);
        }

        public ApplicationSettings GetSettings()
        {
            var settings = _appConfigurationRepository.LoadApplicationSettings();

            var currentSettings = settings.FirstOrDefault();

            if (currentSettings != null)
                CurrentApplicationSettings = currentSettings;

            return currentSettings;
        }

        public async Task<ApplicationSettings> GetSettingsAsync()
        {
            var settings = await _appConfigurationRepository.LoadApplicationSettingsAsync();

            return settings.FirstOrDefault();
        }

        public async Task InitializeMethod()
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

        public async Task SaveBackGroundImage(byte[] array)
        {
            var settings = await GetSettingsAsync();

            settings.BackgroundImage = array;
            await _appConfigurationRepository.SaveApplicationSettingsAsync(settings);
        }

        public async Task SaveConfigurationMethod(Stream stream)
        {
            using (var mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                await _appConfigurationRepository.SaveMethodAsync(new MethodConfiguration { Id = Guid.NewGuid(), SerializedMethod = mem.ToArray() });
            }
        }

        public void SaveSettings(ApplicationSettings settings)
        {
            _appConfigurationRepository.SaveApplicationSettings(settings);
            CurrentApplicationSettings = settings;
        }

        public Task SaveSettingsAsync(ApplicationSettings settings)
        {
            return _appConfigurationRepository.SaveApplicationSettingsAsync(settings);
        }

        public async Task SaveTitle(string title)
        {
            var settings = await GetSettingsAsync();

            settings.Title = title;
            await _appConfigurationRepository.SaveApplicationSettingsAsync(settings);
        }
    }
}