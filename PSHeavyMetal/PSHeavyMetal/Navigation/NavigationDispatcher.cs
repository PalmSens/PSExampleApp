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

        internal static NavigationDispatcher Instance =>
                              _instance ?? (_instance = new NavigationDispatcher());

        internal INavigation Navigation =>
                             _navigation ?? throw new Exception("NavigationDispatcher is not initialized");

        internal static async Task Pop()
        {
            await _instance.Navigation.PopAsync();
        }

        /// <summary>
        /// Pops pages until the root is reached (the home screen)
        /// </summary>
        /// <param name="NavigationViewType"></param>
        /// <returns></returns>
        internal static async Task PopToRoot()
        {
            await _instance.Navigation.PopToRootAsync();
        }

        /// <summary>
        /// Uses the navigation view type to navigate to the page. This makes sure that the view model doesn't have to know about the page
        /// </summary>
        /// <param name="NavigationViewType"></param>
        /// <returns></returns>
        internal static async Task Push(NavigationViewType navigationViewType)
        {
            await _instance.Navigation.PushAsync(PageSelector(navigationViewType));
        }

        internal void Initialize(INavigation navigation)
        {
            _navigation = navigation;
        }

        private static ContentPage PageSelector(NavigationViewType navigationViewType)
        {
            switch (navigationViewType)
            {
                case NavigationViewType.HomeView:
                    return new HomeView();

                case NavigationViewType.PrepareMeasurementView:
                    return new PrepareMeasurementView();

                case NavigationViewType.SelectDeviceView:
                    return new SelectDeviceView();

                case NavigationViewType.ConfigureMeasurementView:
                    return new ConfigureMeasurementView();

                case NavigationViewType.SensorDetectionView:
                    return new SensorDetectionPopup();

                case NavigationViewType.DropDetectionView:
                    return new DropDetectionPopup();

                case NavigationViewType.RunMeasurementView:
                    return new RunMeasurementView();

                case NavigationViewType.MeasurmentFinished:
                    return new MeasurementFinishedView();

                case NavigationViewType.MeasurementDataView:
                    return new MeasurementDataView();

                case NavigationViewType.MeasurementPlotView:
                    return new MeasurementPlotView();

                default:
                    throw new NotImplementedException($"Navigation {navigationViewType} not implemented");
            }
        }
    }
}