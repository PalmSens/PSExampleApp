using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IPermissionService _permissionService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private User _activeUser;
        private User _selectedUser;

        public LoginViewModel(IUserService userService, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _userService = userService;
            _popupNavigation = PopupNavigation.Instance;

            OpenAddUserViewCommand = CommandFactory.Create(async () => await _popupNavigation.PushAsync(new AddUserPopUp()));
            OnPageAppearingCommand = CommandFactory.Create(async () => await UpdateUsers());
            OnPageDisappearingCommand = CommandFactory.Create(async () => await OnDisappearing());
            CancelCommand = CommandFactory.Create(async () => await _popupNavigation.PopAsync());
            SelectionChangedCommand = CommandFactory.Create(SelectionChangedEvent);

            ActiveUser = _userService.ActiveUser;
        }

        public User ActiveUser
        {
            get => _activeUser;
            set => SetProperty(ref _activeUser, value);
        }

        public ICommand CancelCommand { get; }
        public ICommand OnPageAppearingCommand { get; set; }

        public ICommand OnPageDisappearingCommand { get; set; }
        public ICommand OpenAddUserViewCommand { get; }

        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public ICommand SelectionChangedCommand { get; }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public async Task OnDisappearing()
        {
            var bluetoothEnabled = _permissionService.CheckBlueToothEnabled();

            if (!bluetoothEnabled)
            {
                var openSettings = await NavigationDispatcher.PushAlert("Bluetooth", "Please enable bluetooth reader in order to start a measurement");

                if (openSettings)
                    _permissionService.OpenBluetoothSettings();
            }
        }

        private async Task SelectionChangedEvent()
        {
            _userService.SetActiveUser(SelectedUser);

            var code = _userService.GetActiveUserLanguageCode(SelectedUser.Language);
            LocalizationResourceManager.Current.CurrentCulture = code == null ? CultureInfo.CurrentCulture : new CultureInfo(code);

            await _popupNavigation.PopAsync();
        }

        private async Task UpdateUsers()
        {
            foreach (var user in await _userService.GetAllUsersAsync())
            {
                if (!Users.Any(x => x.Name == user.Name))
                    Users.Add(user);
            }
        }
    }
}