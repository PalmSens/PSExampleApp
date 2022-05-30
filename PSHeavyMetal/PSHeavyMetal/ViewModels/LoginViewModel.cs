using MvvmHelpers;
using MvvmHelpers.Commands;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private User _user;

        public LoginViewModel(IUserService userService)
        {
            LoginCommand = new AsyncCommand(OnLoginClicked);
            OpenAddUserViewCommand = new AsyncCommand(OpenAddUserClicked);

            _userService = userService;

            foreach (var user in _userService.GetAllUsers())
            {
                Users.Add(user);
            }
        }

        public AsyncCommand LoginCommand { get; }
        public AsyncCommand OpenAddUserViewCommand { get; }

        public User SelectedUser
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        private async Task OnLoginClicked()
        {
            _userService.SetActiveUser(SelectedUser);
            await NavigationDispatcher.Push(NavigationViewType.SelectDeviceView);
        }

        private async Task OpenAddUserClicked()
        {
            await NavigationDispatcher.Push(NavigationViewType.AddUserView);
        }
    }
}