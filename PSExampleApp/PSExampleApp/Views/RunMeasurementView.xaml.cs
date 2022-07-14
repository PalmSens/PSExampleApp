using PSExampleApp.Forms.ViewModels;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RunMeasurementView : ContentPage
    {
        public RunMeasurementView()
        {
            BindingContext = App.GetViewModel<RunMeasurementViewModel>();

            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}