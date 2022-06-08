using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;
        private User _activeUser;
        private User _selectedUser;

        public LoginViewModel(IUserService userService)
        {
            _userService = userService;
            _popupNavigation = PopupNavigation.Instance;

            LoginCommand = CommandFactory.Create(OnLoginClicked);
            OpenAddUserViewCommand = CommandFactory.Create(async () => await _popupNavigation.PushAsync(new AddUserPopUp()));
            OnPageAppearingCommand = CommandFactory.Create(async () => await UpdateUsers());
            CancelCommand = CommandFactory.Create(async () => await _popupNavigation.PopAsync());

            _userService.ActiveUserChanged += _userService_ActiveUserChanged;

            ActiveUser = _userService.ActiveUser;
        }

        public User ActiveUser
        {
            get => _activeUser;
            set => SetProperty(ref _activeUser, value);
        }

        public ICommand CancelCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand OnPageAppearingCommand { get; set; }

        public ICommand OpenAddUserViewCommand { get; }

        public User SelectedUser
        {
            get => _selectedUser;
            set => SetProperty(ref _selectedUser, value);
        }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        private async void _userService_ActiveUserChanged(object sender, User e)
        {
            await UpdateUsers();

            SelectedUser = e;
            OnPropertyChanged(nameof(SelectedUser));
        }

        private async Task OnLoginClicked()
        {
            _userService.SetActiveUser(SelectedUser);
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