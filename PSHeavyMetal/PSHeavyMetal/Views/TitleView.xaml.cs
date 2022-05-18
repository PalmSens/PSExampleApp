using PSHeavyMetal.Forms.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
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