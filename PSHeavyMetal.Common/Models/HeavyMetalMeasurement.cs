using PalmSens.Core.Simplified.Data;

namespace PSHeavyMetal.Common.Models
{
    public class HeavyMetalMeasurement : DataObject
    {
        public string Description { get; set; }
        public SimpleMeasurement ConfiguredMeasurement { get; set; }
    }
}