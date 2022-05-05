using MvvmHelpers.Commands;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Forms.Views;
using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using MvvmHelpers;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public AsyncCommand LoginCommand { get; }

        public AsyncCommand OpenAddUserViewCommand { get; }

        public ObservableCollection<string> UserNames { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncCommand(OnLoginClicked);
            OpenAddUserViewCommand = new AsyncCommand(OpenAddUserClicked);
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