using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        public HomeView()
        {
            BindingContext = App.GetViewModel<HomeViewModel>();
            InitializeComponent();
        }
    }
}