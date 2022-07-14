using PSExampleApp.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TitleView : ContentView
    {
        public TitleView()
        {
            BindingContext = App.GetViewModel<TitleViewModel>();
            InitializeComponent();
        }
    }
}