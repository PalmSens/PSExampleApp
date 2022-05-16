using PalmSens;

namespace PSHeavyMetal.Common.Models
{
    public class HeavyMetalMeasurement : DataObject
    {
        public string Description { get; set; }
        public Measurement ConfiguredMeasurement { get; set; }
    }
}