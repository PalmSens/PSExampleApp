using MvvmHelpers;
using MvvmHelpers.Commands;
using PSHeavyMetal.Core.Services;
using System.Threading.Tasks;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public AddUserViewModel(IUserService userService)
        {
            _userService = userService;
            AddUserCommand = new AsyncCommand<string>(OnAddUserClicked);
        }

        public AsyncCommand<string> AddUserCommand { get; }

        private async Task OnAddUserClicked(string name)
        {
            await _userService.SaveUserAsync(name);
        }
    }
}