using MvvmHelpers;
using Newtonsoft.Json;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class ConfigureMeasurementViewModel : BaseViewModel
    {
        private IMeasurementService _measurementService;

        public ConfigureMeasurementViewModel(IMeasurementService measurementService)
        {
            _measurementService = measurementService;
            OnConfigSelected = CommandFactory.Create(async conf => await SetConfiguration(conf as MeasurementConfiguration));
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
            ImportAnalyteCommand = CommandFactory.Create(ImportAnalyte);
        }

        public ICommand ImportAnalyteCommand { get; }

        public ObservableCollection<MeasurementConfiguration> MeasurementConfigurations { get; } = new ObservableCollection<MeasurementConfiguration>();

        public ICommand OnConfigSelected { get; }

        public ICommand OnPageAppearingCommand { get; }

        private async Task ImportAnalyte()
        {
            var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] { "applicatin/json" } },
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

                    await _measurementService.SaveMeasurementConfiguration(configuration);

                    MeasurementConfigurations.Add(configuration);
                }
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("Exception occured deserializing the selected json file");
            }
            catch(IOException ex)
            {
                Debug.WriteLine("exception occured with loading the selected file");
            }            
        }

        private async Task OnPageAppearing()
        {
            //If page reappears with configurations we don't want to reload them. This happens when a user returns with file picker
            if (MeasurementConfigurations.Count != 0)
                return;

            var configurations = await _measurementService.LoadAllMeasurementConfigurationsAsync();

            foreach (var config in configurations)
                MeasurementConfigurations.Add(config);
        }

        private async Task SetConfiguration(MeasurementConfiguration configuration)
        {
            _measurementService.CreateMeasurement(configuration);

            await NavigationDispatcher.Push(NavigationViewType.PrepareMeasurementView);
        }
    }
}