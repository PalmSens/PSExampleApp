using PSExampleApp.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurementDataView : ContentPage
    {
        public MeasurementDataView()
        {
            BindingContext = App.GetViewModel<MeasurementDataViewModel>();
            InitializeComponent();
        }
    }
}