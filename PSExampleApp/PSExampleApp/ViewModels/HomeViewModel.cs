using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace PSExampleApp.Forms.ViewModels
{
    public class HomeViewModel : BaseAppViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;

        public HomeViewModel(IUserService userService, IMeasurementService measurementService, IAppConfigurationService appConfigurationService) : base(appConfigurationService)
        {
            _popupNavigation = PopupNavigation.Instance;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
            OpenMeasurementListCommand = CommandFactory.Create(OpenMeasurementList);
            OpenLoginPopupCommand = CommandFactory.Create(OpenLoginPopup);
            StartMeasurementCommand = CommandFactory.Create(StartMeasurement);
            ConfigureApplicationCommand = CommandFactory.Create(ConfigureApplication);
            _measurementService = measurementService;
            _userService = userService;
            MessagingCenter.Subscribe<object>(this, "UpdateSettings", (_) => { OnPropertyChanged(nameof(ActiveUserIsAdmin)); });
        }

        public bool ActiveUserIsAdmin => _userService.ActiveUser?.IsAdmin ?? false;

        public ICommand ConfigureApplicationCommand { get; set; }
        public ICommand OnPageAppearingCommand { get; set; }
        public ICommand OnPageDisappearingCommand { get; set; }
        public ICommand OpenLoginPopupCommand { get; }
        public ICommand OpenMeasurementListCommand { get; }
        public ICommand StartMeasurementCommand { get; }

        public async Task OnPageAppearing()
        {
            await _measurementService.InitializeMeasurementConfigurations();
            _userService.ActiveUserChanged += _userService_ActiveUserChanged;

            if (_userService.ActiveUser == null)
            {
                //This is during the initialization of the project. We check if the popup stack is 0. If its not then it means that the page onappearing is not triggered by the app startup
                if (_popupNavigation.PopupStack.Count == 0)
                {
                    await NavigationDispatcher.Push(NavigationViewType.LoginView);

                    await _appConfigurationService.InitializeMethod();
                }
            }
            MessagingCenter.Send<object>(this, "DiscoverDevices");
        }

        public void OnPageDisappearing()
        {
            _userService.ActiveUserChanged -= _userService_ActiveUserChanged;
        }

        private void _userService_ActiveUserChanged(object sender, Common.Models.User e)
        {
            OnPropertyChanged(nameof(ActiveUserIsAdmin));
        }

        private async Task ConfigureApplication()
        {
            await NavigationDispatcher.Push(NavigationViewType.ConfigureApplicationView);
        }

        private async Task OpenLoginPopup()
        {
            await NavigationDispatcher.Push(NavigationViewType.LoginView);
        }

        private async Task OpenMeasurementList()
        {
            await NavigationDispatcher.Push(NavigationViewType.SelectMeasurementView);
        }

        private async Task StartMeasurement()
        {
            await NavigationDispatcher.Push(NavigationViewType.SelectDeviceView);
        }
    }
}