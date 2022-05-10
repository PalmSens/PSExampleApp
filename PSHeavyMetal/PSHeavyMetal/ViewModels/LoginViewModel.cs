using MvvmHelpers.Commands;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Forms.Views;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using PSHeavyMetal.Core.Services;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private User _user;

        public AsyncCommand LoginCommand { get; }
        public AsyncCommand OpenAddUserViewCommand { get; }

        public ObservableCollection<User> Users { get; } = new ObservableCollection<User>();

        public User SelectedUser
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

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

        private async Task OpenAddUserClicked()
        {
            await Shell.Current.GoToAsync($"{nameof(AddUserView)}");
        }

        private async Task OnLoginClicked()
        {
            await Shell.Current.GoToAsync($"//{nameof(SelectDeviceView)}");
        }
    }
}