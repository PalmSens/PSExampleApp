using PSExampleApp.Forms.Resx;
using PSExampleApp.Forms.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PSExampleApp.Forms.Navigation
{
    internal class NavigationDispatcher
    {
        private static NavigationDispatcher _instance;
        private INavigation _navigation;

        internal static NavigationDispatcher Instance =>
                              _instance ?? (_instance = new NavigationDispatcher());

        internal INavigation Navigation =>
                             _navigation ?? throw new Exception("NavigationDispatcher is not initialized");

        internal static async Task Pop()
        {
            await _instance.Navigation.PopAsync(Device.RuntimePlatform == Device.iOS ? false : true);
        }

        /// <summary>
        /// Pops pages until the root is reached (the home screen)
        /// </summary>
        /// <param name="NavigationViewType"></param>
        /// <returns></returns>
        internal static async Task PopToRoot()
        {
            await _instance.Navigation.PopToRootAsync(Device.RuntimePlatform == Device.iOS ? false : true);
        }

        /// <summary>
        /// Uses the navigation view type to navigate to the page. This makes sure that the view model doesn't have to know about the page
        /// </summary>
        /// <param name="NavigationViewType"></param>
        /// <returns></returns>
        internal static async Task Push(NavigationViewType navigationViewType)
        {
            await _instance.Navigation.PushAsync(PageSelector(navigationViewType), Device.RuntimePlatform == Device.iOS ? false : true);
        }

        internal static Task<bool> PushAlert(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, AppResources.Ok, AppResources.Cancel);
        }

        internal void Initialize(INavigation navigation)
        {
            _navigation = navigation;
        }

        /// <summary>
        /// Translates page enum to page instance
        /// </summary>
        /// <param name="navigationViewType"></param>
        /// <returns></returns>
        private static ContentPage PageSelector(NavigationViewType navigationViewType)
        {
            Type pageType = Type.GetType($"PSExampleApp.Forms.Views.{navigationViewType}");
            if(pageType == null)
            {
                throw new NotImplementedException($"Navigation {navigationViewType} not implemented");
            }
            return Activator.CreateInstance(pageType) as ContentPage;
        }
    }
}