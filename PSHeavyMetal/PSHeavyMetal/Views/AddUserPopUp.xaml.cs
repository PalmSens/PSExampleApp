using PSHeavyMetal.Forms.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserPopUp : PopupPage
    {
        public AddUserPopUp()
        {
            BackgroundColor = new Xamarin.Forms.Color(0, 0, 0, 0.8);
            BindingContext = App.GetViewModel<AddUserViewModel>();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.UserNameEntry.Focus();
        }
    }
}