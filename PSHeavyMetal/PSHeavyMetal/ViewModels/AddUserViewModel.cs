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
            AddUserCommand = new AsyncCommand(OnAddUserClicked);
        }

        public AsyncCommand AddUserCommand { get; }
        public string UserName { get; set; }

        public async Task OnAddUserClicked()
        {
            await _userService.SaveUserAsync(UserName);
        }
    }
}