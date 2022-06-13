namespace PSHeavyMetal.Common.Models
{
    public class ConcentrationMethod
    {
        /// <summary>
        /// Gets or sets the voltage where the peak is expected to be
        /// </summary>
        public double ExpectedPeakOnXAxis { get; set; }

        /// <summary>
        /// The y background value of a linear calculation
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets the search distance of the peak.
        /// </summary>
        public double PeakWidth { get; set; }

        /// <summary>
        /// The slope value of a linear calculation
        /// </summary>
        public double Slope { get; set; }
    }
}