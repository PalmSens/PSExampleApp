﻿using MvvmHelpers;
using Newtonsoft.Json;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSExampleApp.Forms.ViewModels
{
    public class ConfigureMeasurementViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IMessageService _messageService;

        public ConfigureMeasurementViewModel(IMeasurementService measurementService, IMessageService messageService)
        {
            this._measurementService = measurementService;
            this._messageService = messageService;
            this.OnConfigSelectedCommand = CommandFactory.Create(async conf => await SetConfiguration(conf as MeasurementConfiguration));
            this.OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
            this.ImportAnalyteCommand = CommandFactory.Create(ImportAnalyte);
            this.DeleteConfigurationCommand = CommandFactory.Create(async conf => await DeleteConfiguration(conf as MeasurementConfiguration));
        }

        public ICommand DeleteConfigurationCommand { get; }

        /// <summary>
        /// Gets the import analyte command where the user can select a analyte to use for measurements
        /// </summary>
        public ICommand ImportAnalyteCommand { get; }

        public ObservableCollection<MeasurementConfiguration> MeasurementConfigurations { get; } = new ObservableCollection<MeasurementConfiguration>();

        public ICommand OnConfigSelectedCommand { get; }

        public ICommand OnPageAppearingCommand { get; }

        private async Task DeleteConfiguration(MeasurementConfiguration config)
        {
            try
            {
                await _measurementService.DeleteMeasurementConfiguration(config.Id);
                this.MeasurementConfigurations.Remove(config);
            }
            catch (Exception)
            {
                _messageService.LongAlert("Failed deleting the analyte. Please restart the application if the problem persists");
            }
        }

        private async Task ImportAnalyte()
        {
            var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] { "application/json" } },
                { DevicePlatform.Android, new[] { "application/json" } },
                });
            var options = new PickOptions
            {
                FileTypes = customFileType,
            };

            try
            {
                var result = await FilePicker.PickAsync(options);

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var streamReader = new StreamReader(stream);

                    var jsonString = await streamReader.ReadToEndAsync();
                    var configuration = JsonConvert.DeserializeObject<MeasurementConfiguration>(jsonString);

                    await this._measurementService.SaveMeasurementConfiguration(configuration);

                    this.MeasurementConfigurations.Add(configuration);
                }
            }
            catch (PermissionException)
            {
                _messageService.LongAlert("Failed to import file. Permissions not set");
            }
            catch (Exception)
            {
                _messageService.LongAlert("Failed importing analyte. Please check if the json file has the correct format");
            }
        }

        private async Task OnPageAppearing()
        {
            // If page reappears with configurations we don't want to reload them. This happens when a user returns with file picker
            if (this.MeasurementConfigurations.Count != 0)
                return;

            var configurations = await this._measurementService.LoadAllMeasurementConfigurationsAsync();

            foreach (var config in configurations)
                this.MeasurementConfigurations.Add(config);
        }

        private async Task SetConfiguration(MeasurementConfiguration configuration)
        {
            this._measurementService.CreateMeasurement(configuration);

            await NavigationDispatcher.Push(NavigationViewType.PrepareMeasurementView);
        }
    }
}