﻿using MvvmHelpers;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IAppConfigurationService _appConfigurationService;
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;

        public HomeViewModel(IUserService userService, IMeasurementService measurementService, IAppConfigurationService appConfigurationService)
        {
            _popupNavigation = PopupNavigation.Instance;
            _appConfigurationService = appConfigurationService;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
            OpenMeasurementListCommand = CommandFactory.Create(OpenMeasurementList);
            OpenLoginPopupCommand = CommandFactory.Create(OpenLoginPopup);
            StartMeasurementCommand = CommandFactory.Create(StartMeasurement);
            ConfigureApplicationCommand = CommandFactory.Create(ConfigureApplication);
            _measurementService = measurementService;
            _userService = userService;
        }

        public bool ActiveUserIsAdmin => _userService.ActiveUser.IsAdmin;

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
                    await _popupNavigation.PushAsync(new LoginPopUp());

                    await _appConfigurationService.InitializeMethod();
                }
            }
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
            await _popupNavigation.PushAsync(new LoginPopUp());
        }

        private async Task OpenMeasurementList()
        {
            await _popupNavigation.PushAsync(new SelectMeasurementPopup());
        }

        private async Task StartMeasurement()
        {
            await NavigationDispatcher.Push(NavigationViewType.SelectDeviceView);
        }
    }
}