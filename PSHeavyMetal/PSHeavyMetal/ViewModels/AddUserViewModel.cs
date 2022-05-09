using MvvmHelpers;
using MvvmHelpers.Commands;
using PSHeavyMetal.Core.Services;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public AddUserViewModel(IUserService userService)
        {
            _userService = userService;
            AddUserCommand = new AsyncCommand(OnAddUserClicked);
            CancelCommand = new AsyncCommand(OnCancelClicked);
        }

        public AsyncCommand AddUserCommand { get; }
        public AsyncCommand CancelCommand { get; }
        public string UserName { get; set; }

        public async Task OnAddUserClicked()
        {
            await _userService.SaveUserAsync(UserName);
            await Shell.Current.GoToAsync($"..");
        }

        public async Task OnCancelClicked()
        {
            await Shell.Current.GoToAsync($"..");
        }
    }
}