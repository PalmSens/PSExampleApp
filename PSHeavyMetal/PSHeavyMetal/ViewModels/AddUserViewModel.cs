using MvvmHelpers;
using PSHeavyMetal.Core.Services;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;

        public AddUserViewModel(IUserService userService)
        {
            _popupNavigation = PopupNavigation.Instance;
            _userService = userService;
            AddUserCommand = CommandFactory.Create(OnAddUserClicked);
        }

        public ICommand AddUserCommand { get; }
        public string UserName { get; set; }

        public async Task OnAddUserClicked()
        {
            await _userService.SaveUserAsync(UserName);
            await _popupNavigation.PopAsync();
        }
    }
}