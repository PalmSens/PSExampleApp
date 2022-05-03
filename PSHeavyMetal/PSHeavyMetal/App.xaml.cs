using Android.Content.Res;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Startup.Init();
            MainPage = new MainPage();
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