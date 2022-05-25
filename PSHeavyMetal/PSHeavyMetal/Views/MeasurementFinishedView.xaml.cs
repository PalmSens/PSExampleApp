using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
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