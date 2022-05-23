using PSSDKXamarinFormsTemplateApp.ViewModels;

namespace PSSDKXamarinFormsTemplateApp.Services
{
    public static class NavigationService
    {
        public static void NavigateToView(string view)
        {
            if (App.Current.MainPage.BindingContext is MainViewModel mainViewModel)
            {
                mainViewModel.CurrentView = view;
            }
        }
    }
}