using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
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