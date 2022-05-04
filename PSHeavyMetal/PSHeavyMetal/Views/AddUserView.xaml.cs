using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddUserView : ContentPage
    {
        public AddUserView()
        {
            InitializeComponent();
            BindingContext = App.GetViewModel<AddUserViewModel>();
        }
    }
}