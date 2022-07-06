using PalmSens;
using PalmSens.Core.Simplified.Data;
using PSHeavyMetal.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static PalmSens.Core.Simplified.PSCommSimple;

namespace PSHeavyMetal.Core.Services
{
    public interface IMeasurementService
    {
        public event SimpleCurveStartReceivingDataHandler DataReceived;

        /// <summary>
        /// Triggers when the active measurement is changed
        /// </summary>
        public event EventHandler<HeavyMetalMeasurement> MeasurementChanged;

        public event EventHandler MeasurementEnded;

        public event EventHandler MeasurementStarted;

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        /// <summary>
        /// Calculations based on the configuration of the caluclation method
        /// </summary>
        public void CalculateConcentration();

        /// <summary>
        /// Creates a measurement and sets it as active
        /// </summary>
        /// <returns></returns>
        public HeavyMetalMeasurement CreateMeasurement(MeasurementConfiguration configuration);

        /// <summary>
        /// Deletes a measurement from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteMeasurement(Guid id);

        /// <summary>
        /// Deletes a measurement configuration from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task DeleteMeasurementConfiguration(Guid id);

        /// <summary>
        /// When there are no configurations present in the database then it will initiliaze the default measurements.
        /// /// </summary>
        /// <returns></returns>
        public Task InitializeMeasurementConfigurations();

        /// <summary>
        /// Loads all the measurement configurations in the database
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<MeasurementConfiguration>> LoadAllMeasurementConfigurationsAsync();

        /// <summary>
        /// Loads a specific measurement with the measurement id and also sets this measurement as active measurement
        /// </summary>
        /// <returns></returns>
        public Task<HeavyMetalMeasurement> LoadMeasurement(Guid id);

        /// <summary>
        /// Loads the measurement configuration from a json file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public Task<MeasurementConfiguration> LoadMeasurementConfigurationFromFile(string filename);

        public Method LoadMethod(string filename);

        /// <summary>
        /// Resets the measurement this means setting the active measurement to null
        /// </summary>
        public void ResetMeasurement();

        /// <summary>
        /// Saves the measurement
        /// </summary>
        /// <param name="measurement"></param>
        /// <returns></returns>
        public Task SaveMeasurement(HeavyMetalMeasurement measurement);

        /// <summary>
        /// Saves the measurement configuration. This can be from a filepicker
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public Task SaveMeasurementConfiguration(MeasurementConfiguration configuration);

        public Task<SimpleMeasurement> StartMeasurement(Method method);
    }
}