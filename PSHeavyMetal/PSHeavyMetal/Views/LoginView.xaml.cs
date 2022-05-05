using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        private LoginViewModel _viewModel;

        public LoginView()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<LoginViewModel>();
            _viewModel = (LoginViewModel)App.GetViewModel<LoginViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}