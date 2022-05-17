using PSHeavyMetal.Forms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DropDetectionView : ContentPage
    {
        public DropDetectionView()
        {
            BindingContext = App.GetViewModel<DropDetectionViewModel>();
            InitializeComponent();
        }
    }
}