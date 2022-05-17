using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SensorDetectionView : ContentPage
    {
        public SensorDetectionView()
        {
            BindingContext = App.GetViewModel<SensorDetectionViewModel>();
            InitializeComponent();
        }
    }
}