using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusBarView : ContentView
    {
        public StatusBarView()
        {
            BindingContext = App.GetViewModel<StatusBarViewModel>();
            InitializeComponent();
        }
    }
}