using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConfigureApplicationView : ContentPage
    {
        public ConfigureApplicationView()
        {
            BindingContext = App.GetViewModel<ConfigureApplicationViewModel>();
            InitializeComponent();
        }
    }
}