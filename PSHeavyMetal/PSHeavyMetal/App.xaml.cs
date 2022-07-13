using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Resx;
using PSHeavyMetal.Forms.Views;
using System;
using System.IO;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms
{
    public partial class App : Application
    {
        private static IServiceProvider ServiceProvider;

        public App()
        {
            ServiceProvider = Startup.Init();
            InitializeComponent();

            LocalizationResourceManager.Current.PropertyChanged += Current_PropertyChanged;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

            var appConfigurationService = ServiceProvider.GetService<IAppConfigurationService>();

            var settings = appConfigurationService.GetSettings();

            if (settings == null)
            {
                using var backgroundImageStream = appConfigurationService.GetBackgroundImage();

                using (var mem = new MemoryStream())
                {
                    backgroundImageStream.BaseStream.CopyTo(mem);
                    settings = new ApplicationSettings { Title = "PS Heavy Metal", Id = Guid.NewGuid(), BackgroundImage = mem.ToArray() };
                    appConfigurationService.SaveSettings(settings);
                }
            }

            var navigationPage = new NavigationPage(new HomeView())
            {
                BarBackgroundColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
            };

            MainPage = new CustomFlyOutPage()
            {
                Flyout = new MainMenuView(),
                Detail = navigationPage,
                BackgroundImageSource = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(settings.BackgroundImage);
                }),
            };

            NavigationDispatcher.Instance.Initialize(navigationPage.Navigation);
        }

        public static BaseViewModel GetViewModel<T>() where T : BaseViewModel => ServiceProvider.GetService<T>();

        protected override void OnResume()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnStart()
        {
        }

        private void Current_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            AppResources.Culture = LocalizationResourceManager.Current.CurrentCulture;
        }
    }
}