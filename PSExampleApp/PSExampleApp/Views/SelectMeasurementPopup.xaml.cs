using PSExampleApp.Forms.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectMeasurementPopup : PopupPage
    {
        public SelectMeasurementPopup()
        {
            BackgroundColor = new Xamarin.Forms.Color(0, 0, 0, 0.9);
            BindingContext = App.GetViewModel<SelectMeasurementViewModel>();
            InitializeComponent();
        }
    }
}