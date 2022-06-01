using PalmSens.Core.Simplified.Data;

namespace PSHeavyMetal.Common.Models
{
    public class HeavyMetalMeasurement : DataObject
    {
        /// <summary>
        /// The configured heavy metal
        /// </summary>
        public MeasurementConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets the measurement which is configured in the simplified core
        /// </summary>
        public SimpleMeasurement Measurement { get; set; }
    }
}