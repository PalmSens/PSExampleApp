using System.Collections.Generic;

namespace PSExampleApp.Common.Models
{
    public class User : DataObject
    {
        public bool IsAdmin { get; set; }
        public List<MeasurementInfo> Measurements { get; set; } = new List<MeasurementInfo>();
        public string Password { get; set; }
    }
}