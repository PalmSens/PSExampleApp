using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private User _user;

        public LoginViewModel(IUserService userService)
        {
            LoginCommand = CommandFactory.Create(OnLoginClicked);
            OpenAddUserViewCommand = CommandFactory.Create(OpenAddUserClicked);
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);

            _userService = userService;
        }

        public ICommand LoginCommand { get; }
        public ICommand OnPageAppearingCommand { get; set; }
        public ICommand OpenAddUserViewCommand { get; }

        public User SelectedUser
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public async Task OnPageAppearing()
        {
            SelectedUser = _userService.ActiveUser;

            foreach (var user in await _userService.GetAllUsersAsync())
            {
                if (!Users.Any(x => x.Name == user.Name))
                    Users.Add(user);
            }            
        }

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