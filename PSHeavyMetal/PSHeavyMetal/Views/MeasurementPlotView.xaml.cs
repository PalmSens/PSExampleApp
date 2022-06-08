using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeasurementPlotView : ContentPage
    {
        public MeasurementPlotView()
        {
            BindingContext = App.GetViewModel<MeasurementPlotViewModel>();
            InitializeComponent();
        }
    }
}