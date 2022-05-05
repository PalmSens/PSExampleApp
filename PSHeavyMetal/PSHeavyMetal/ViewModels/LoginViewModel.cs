using MvvmHelpers.Commands;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Forms.Views;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MvvmHelpers;
using PSHeavyMetal.Core.Services;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public AsyncCommand LoginCommand { get; }
        public AsyncCommand OpenAddUserViewCommand { get; }
        public AsyncCommand RetrieveUsersCommand { get; }

        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        public User SelectedUser { get; set; }

        public LoginViewModel(IUserService userService)
        {
            LoginCommand = new AsyncCommand(OnLoginClicked);
            OpenAddUserViewCommand = new AsyncCommand(OpenAddUserClicked);
            RetrieveUsersCommand = new AsyncCommand(RetrieveUsers);

            _userService = userService;
        }

        public async void OnAppearing()
        {
            await this.RetrieveUsers();
        }

        private async Task RetrieveUsers()
        {
            Users = new ObservableCollection<User>(await _userService.GetAllUsersAsync());
        }

        private async Task OpenAddUserClicked()
        {
            await Shell.Current.GoToAsync($"{nameof(AddUserView)}");
        }

        private Task OnLoginClicked()
        {
            throw new NotImplementedException();
        }
    }
}