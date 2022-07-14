using System;

namespace PSExampleApp.Common.Models
{
    /// <summary>
    /// This class is used to display a list of measurements. The user can then select a option and the real measurement will load    ///
    /// </summary>
    public class MeasurementInfo : DataObject
    {
        public DateTime MeasurementDate { get; set; }
    }
}