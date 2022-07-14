using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PSExampleApp.Common.Models
{
    /// <summary>
    /// This class represents the measurement configuration of a heavy metal measurement
    /// </summary>
    public class MeasurementConfiguration : DataObject
    {
        /// <summary>
        /// The name of the analyte
        /// </summary>
        public string AnalyteName { get; set; }

        /// <summary>
        /// Gets or sets the calculation values of the concentration
        /// </summary>
        public ConcentrationMethod ConcentrationMethod { get; set; }

        /// <summary>
        /// Gets or sets the concentration unit
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public ConcentrationUnit ConcentrationUnit { get; set; }

        /// <summary>
        /// Gets or sets the descriptions which is based on the input of the user
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets whether the measurement configuration is default or not. If its default it cannot be deleted.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}