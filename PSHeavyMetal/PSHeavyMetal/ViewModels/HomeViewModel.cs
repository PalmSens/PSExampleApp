using MvvmHelpers;
using PSHeavyMetal.Common.Models;
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
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private User _user;

        public HomeViewModel(IUserService userService)
        {
            _popupNavigation = PopupNavigation.Instance;

            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
            OpenMeasurementListCommand = CommandFactory.Create(OpenMeasurementList);
            OpenLoginPopupCommand = CommandFactory.Create(OpenLoginPopup);
            StartMeasurementCommand = CommandFactory.Create(StartMeasurement);
            _userService = userService;
        }

        public ICommand OnPageAppearingCommand { get; set; }

        public ICommand OpenLoginPopupCommand { get; }

        public ICommand OpenMeasurementListCommand { get; }

        public ICommand StartMeasurementCommand { get; }

        public async Task OnPageAppearing()
        {
            if (_userService.ActiveUser == null)
                await _popupNavigation.PushAsync(new LoginPopUp());
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