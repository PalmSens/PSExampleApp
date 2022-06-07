using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
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