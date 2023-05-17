
using Microsoft.Extensions.DependencyInjection;
using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resources;
using PSExampleApp.Forms.Resx;
using PSExampleApp.Forms.ViewModels;
using PSExampleApp.Forms.Views;
using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSExampleApp.Forms
{
    public partial class App : Application
    {
        private static IServiceProvider ServiceProvider;
        private const string DefaultBackground = "PSExampleApp.Forms.Resources.background.jpeg";

        public App()
        {
            ServiceProvider = Startup.Init();

            LocalizationResourceManager.Current.PropertyChanged += Current_PropertyChanged;
            LocalizationResourceManager.Current.Init(AppResources.ResourceManager);

            var appConfigurationService = ServiceProvider.GetService<IAppConfigurationService>();

            var applicationSettings = appConfigurationService.GetSettings();

            if (applicationSettings == null)
            {
                applicationSettings = new ApplicationSettings
                {
                    Title = "PS Example App", 
                    Id = Guid.NewGuid(), 
                    BackgroundImage = ResourceHelper.GetImageAsByteArray(DefaultBackground)
                };
                appConfigurationService.SaveSettings(applicationSettings);
            }
            else
            {
                if (applicationSettings.ActiveUserId.HasValue)
                {
                    var userService = ServiceProvider.GetService<IUserService>();
                    Task.Run(async () => await userService.LoadUserAsync(applicationSettings.ActiveUserId.Value));
                }
            }


            InitializeComponent();

            var navigationPage = new CustomNavigationPage(new HomeView() { BackgroundImageSource = ImageSource.FromStream(() => { return new MemoryStream(applicationSettings.BackgroundImage); }), })
            {
                BarBackgroundColor = Color.Transparent,
                BackgroundColor = Color.Transparent,
            };

            MainPage = navigationPage;

            NavigationDispatcher.Instance.Initialize(navigationPage.Navigation);
        }

        public static BaseAppViewModel GetViewModel<T>() where T : BaseAppViewModel => ServiceProvider.GetService<T>();

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