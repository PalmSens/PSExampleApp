using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.ViewModels
{
    internal class ViewViewModel : BaseViewModel
    {
        protected MainViewModel MainViewModel { get; }

        public ViewViewModel()
        {
            MainViewModel = DependencyService.Resolve<MainViewModel>();
        }
    }
}