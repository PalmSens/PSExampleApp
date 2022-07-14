using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
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
            return false;
        }
    }
}