using PSHeavyMetal.Forms.Views;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.Navigation
{
    internal class NavigationDispatcher
    {
        private static NavigationDispatcher _instance;

        private INavigation _navigation;

        /// <summary>
        /// Uses the navigation view type to navigate to the page. This makes sure that the view model doesn't have to know about the page
        /// </summary>
        /// <param name="NavigationViewType"></param>
        /// <returns></returns>
        internal static async Task Push(NavigationViewType navigationViewType)
        {
            await _instance.Navigation.PushAsync(PageSelector(navigationViewType));
        }

        internal static async Task Pop()
        {
            await _instance.Navigation.PopAsync();
        }

        private static ContentPage PageSelector(NavigationViewType navigationViewType)
        {
            switch (navigationViewType)
            {
                case NavigationViewType.LoginView:
                    return new LoginView();

                case NavigationViewType.AddUserView:
                    return new AddUserView();

                case NavigationViewType.PrepareMeasurementView:
                    return new PrepareMeasurementView();

                case NavigationViewType.SelectDeviceView:
                    return new SelectDeviceView();

                case NavigationViewType.ConfigureMeasurementView:
                    return new ConfigureMeasurementView();

                case NavigationViewType.SensorDetectionView:
                    return new SensorDetectionView();

                default:
                    throw new NotImplementedException($"Navigation {navigationViewType} not implemented");
            }
        }

        internal static NavigationDispatcher Instance =>
                      _instance ?? (_instance = new NavigationDispatcher());

        internal INavigation Navigation =>
                     _navigation ?? throw new Exception("NavigationDispatcher is not initialized");

        internal void Initialize(INavigation navigation)
        {
            _navigation = navigation;
        }
    }
}