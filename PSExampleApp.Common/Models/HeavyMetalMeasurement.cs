namespace PSExampleApp.Common.Models
{
    using PalmSens.Core.Simplified.Data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The measurement of a heavy metal consisting of result, configuration and simplemeasurement used by the simple wrapper
    /// </summary>
    public class HeavyMetalMeasurement : DataObject
    {
        /// <summary>
        /// Gets or sets the concentration amount which is calculated based on the found peaks and the concentration method
        /// </summary>
        public double Concentration { get; set; }

        /// <summary>
        /// Gets or sets the configured heavy metal measurement
        /// </summary>
        public MeasurementConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the measurement which is configured in the simplified core
        /// </summary>
        public SimpleMeasurement Measurement { get; set; }

        /// <summary>
        /// Gets or sets the date of the measurement
        /// </summary>
        public DateTime MeasurementDate { get; set; }

        /// <summary>
        /// Gets or sets the serialized measurement images
        /// </summary>
        public List<byte[]> MeasurementImages { get; set; } = new List<byte[]>();
    }
}