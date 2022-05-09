using Microsoft.Extensions.DependencyInjection;
using PSHeavyMetal.Forms.ViewModels;
using System;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms
{
    public partial class App : Application
    {
        private static IServiceProvider ServiceProvider;

        public App()
        {
            InitializeComponent();
            ServiceProvider = Startup.Init();
            MainPage = new AppShell();
        }

        public static BaseViewModel GetViewModel<T>() where T : BaseViewModel => ServiceProvider.GetService<T>();

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