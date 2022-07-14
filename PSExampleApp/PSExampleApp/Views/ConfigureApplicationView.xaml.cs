using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
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