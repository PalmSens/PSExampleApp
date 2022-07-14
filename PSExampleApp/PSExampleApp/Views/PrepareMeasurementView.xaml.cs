using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PrepareMeasurementView : ContentPage
    {
        public PrepareMeasurementView()
        {
            BindingContext = App.GetViewModel<PrepareMeasurementViewModel>();
            InitializeComponent();
        }
    }
}