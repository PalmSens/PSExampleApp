using Xamarin.Forms;

namespace PSExampleApp.Common.Models
{
    /// <summary>
    /// This class is for displaying the photos in the measurement data and measurement finished view. This class is necessary for the collectionview binding
    /// </summary>
    public class MeasurementPhotoPresenter
    {
        public ImageSource Photo { get; set; }
    }
}