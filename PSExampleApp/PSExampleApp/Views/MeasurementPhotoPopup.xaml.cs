using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurementPhotoPopup : PopupPage
    {
        public MeasurementPhotoPopup(ImageSource image)
        {
            InitializeComponent();
            ImageDisplay.Source = image;
        }
    }
}