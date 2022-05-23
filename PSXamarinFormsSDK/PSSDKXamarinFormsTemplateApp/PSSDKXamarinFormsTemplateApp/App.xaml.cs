using System;
using PSSDKXamarinFormsTemplateApp.Pages;
using PSSDKXamarinFormsTemplateApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSSDKXamarinFormsTemplateApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //var mainViewModel = new MainViewModel();
            //DependencyService.RegisterSingleton(mainViewModel);
            MainPage = new MainPage();
            //MainPage.BindingContext = mainViewModel;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
