using PSHeavyMetal.Forms.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPopUp : PopupPage
    {
        public LoginPopUp()
        {
            BackgroundColor = new Xamarin.Forms.Color(0, 0, 0, 0.9);
            BindingContext = App.GetViewModel<LoginViewModel>();
            InitializeComponent();
        }
    }
}