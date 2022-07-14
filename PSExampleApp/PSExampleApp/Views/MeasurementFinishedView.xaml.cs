using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurementFinishedView : ContentPage
    {
        public MeasurementFinishedView()
        {
            BindingContext = App.GetViewModel<MeasurementFinishedViewModel>();
            InitializeComponent();
        }
    }
}