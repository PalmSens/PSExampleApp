using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Styles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Generic : ResourceDictionary
    {
        public Generic()
        {
            App.Current.Resources.MergedDictionaries.Add(new Colors());
            App.Current.Resources.MergedDictionaries.Add(new Fonts());
            InitializeComponent();
        }
    }
}