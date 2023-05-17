using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resx;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class LoginViewModel : BaseAppViewModel
    {
        private readonly IPermissionService _permissionService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private User _activeUser;
        private User _selectedUser;
        private string _newUserName;

        public LoginViewModel(
            IAppConfigurationService appConfigurationService,
            IUserService userService,
            IPermissionService permissionService,
            IMessageService messageService) : base(appConfigurationService)
        {    
            _permissionService = permissionService;
            _userService = userService;
            _popupNavigation = PopupNavigation.Instance;
            _messageService = messageService;

            OpenAddUserViewCommand = CommandFactory.Create(async () => await AddNewUser()); ;
            OnPageAppearingCommand = CommandFactory.Create(OnAppearing);
            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
            CancelCommand = CommandFactory.Create(async () => await NavigationDispatcher.Pop());
            SelectionChangedCommand = CommandFactory.Create(SelectionChangedEvent);
            GetUserCommand = new AsyncCommand<User>(async (u) => await ShowUserDetails(u), allowsMultipleExecutions: false);
            RemoveUserCommand = new AsyncCommand<User>(async (u) => await RemoveUser(u), allowsMultipleExecutions: false);

            ActiveUser = _userService.ActiveUser;
        }

        private async Task AddNewUser()
        {
            if (string.IsNullOrEmpty(NewUserName))
            {
                _messageService.ShortAlert(AppResources.Alert_ValidUserName);
                return;
            }

            try
            {
                await _userService.SaveUserAsync(NewUserName);

                var applicationSettings = await _appConfigurationService.GetSettingsAsync();
                applicationSettings.ActiveUserId = _userService.ActiveUser.Id;
                await _appConfigurationService.SaveSettingsAsync(applicationSettings);

                await NavigationDispatcher.Pop();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _messageService.ShortAlert(AppResources.Alert_ValidUserName);
            }
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

        public string NewUserName
        {
            get => _newUserName;
            set => SetProperty(ref _newUserName, value);
        }

        public ICommand SelectionChangedCommand { get; }
        public IAsyncCommand<User> GetUserCommand { get; set; }
        public IAsyncCommand<User> RemoveUserCommand { get; set; }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();


        public async Task OnAppearing()
        {
            var applicationSettings = await _appConfigurationService.GetSettingsAsync();

            ActiveUser = null;
            if (applicationSettings?.ActiveUserId != null)
            {
                ActiveUser = await _userService.LoadUserAsync(applicationSettings.ActiveUserId.Value);
            }

            Users.CollectionChanged += Users_CollectionChanged;
            await UpdateUsers();
        }

        public async Task OnDisappearing()
        {
            Users.CollectionChanged -= Users_CollectionChanged;

            var blueToothEnabled = _permissionService.CheckBlueToothEnabled();

            if (!blueToothEnabled)
            {
                var openSettings = await NavigationDispatcher.PushAlert(AppResources.Alert_BluetoothTitle, AppResources.Alert_BluetoothDescription);

                if (openSettings)
                {
                    _permissionService.OpenBluetoothSettings();
                }
            }
        }

        private async Task ShowUserDetails(User selectedUser)
        {
            SelectedUser = selectedUser;
            await SelectionChangedEvent();
        }

        private async Task RemoveUser(User removedUser)
        {
            if (ActiveUser.Id == removedUser.Id)
            {
                _messageService.LongAlert(AppResources.AlertActiveUserCannotBeDeleted);
                return;
            }
            var deleteConfirmed = await NavigationDispatcher.PushAlert(AppResources.AlertRemoveUserTitle, AppResources.AlertRemoveUserDescription);
            if (deleteConfirmed)
            {
                await _userService.DeleteUserAsync(removedUser.Id);
                Users.Remove(removedUser);
            }
        }

        private async Task SelectionChangedEvent()
        {
            _userService.SetActiveUser(SelectedUser);

            var applicationSettings = await _appConfigurationService.GetSettingsAsync();
            applicationSettings.ActiveUserId = SelectedUser.Id;
            await _appConfigurationService.SaveSettingsAsync(applicationSettings);

            await NavigationDispatcher.Pop();
        }

        private async Task UpdateUsers()
        {
            OnPropertyChanged(nameof(HasUser));

            foreach (var user in await _userService.GetAllUsersAsync())
            {
                if (Users.Any(x => x.Name == user.Name)) continue;
                Users.Add(user);
            }
            OnPropertyChanged(nameof(Users));
        }

        private void Users_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(HasUser));
        }
    }
}