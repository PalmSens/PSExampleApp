using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigureAnalyteView : ContentPage
    {
        public ConfigureAnalyteView()
        {
            BindingContext = App.GetViewModel<ConfigureMeasurementViewModel>();
            InitializeComponent();
        }
    }
}