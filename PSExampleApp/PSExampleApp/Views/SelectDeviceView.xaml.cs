using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectDeviceView : ContentPage
    {
        public SelectDeviceView()
        {
            BindingContext = App.GetViewModel<SelectDeviceViewModel>();
            InitializeComponent();
        }
    }
}