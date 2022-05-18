using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomFlyOutPage : FlyoutPage
    {
        public CustomFlyOutPage()
        {
            InitializeComponent();
        }

        public override bool ShouldShowToolbarButton()
        {
            return true;
        }
    }
}