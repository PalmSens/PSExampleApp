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
            OnPageAppearingCommand = CommandFactory.Create(OnAppearing);
            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
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

        public bool HasUser => Users.Count > 0;

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

        public async Task OnAppearing()
        {
            Users.CollectionChanged += Users_CollectionChanged;
            await UpdateUsers();
        }

        public async Task OnDisappearing()
        {
            Users.CollectionChanged -= Users_CollectionChanged;

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
            OnPropertyChanged(nameof(HasUser));

            foreach (var user in await _userService.GetAllUsersAsync())
            {
                if (!Users.Any(x => x.Name == user.Name))
                    Users.Add(user);
            }
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasUser));
        }
    }
}