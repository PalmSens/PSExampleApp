namespace PSExampleApp.Common.Models
{
    public class ConcentrationMethod
    {
        /// <summary>
        /// The y background value of a linear calculation
        /// </summary>
        public int CalibrationCurveOffset { get; set; }

        /// <summary>
        /// The slope value of a linear calculation
        /// </summary>
        public double CalibrationCurveSlope { get; set; }

        /// <summary>
        /// Gets or sets the minimum height the peak should be for peak detection
        /// </summary>
        public double PeakMinHeight { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of the peak width for peak detection
        /// </summary>
        public double PeakMinWidth { get; set; }

        /// <summary>
        /// Gets or sets the maximum value of where the peak should be on the x axis. Together with the peak window x max should create the window
        /// </summary>
        public double PeakWindowXMax { get; set; }

        /// <summary>
        /// Gets or sets the minimum value of where the peak should be on the x axis. Together with the peak window x max should create the window
        /// </summary>
        public double PeakWindowXMin { get; set; }
    }
}