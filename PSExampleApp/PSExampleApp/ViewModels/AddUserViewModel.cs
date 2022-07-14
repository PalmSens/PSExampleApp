using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Core.Services;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class AddUserViewModel : BaseViewModel
    {
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;

        public AddUserViewModel(IUserService userService, IMessageService messageService)
        {
            _messageService = messageService;
            _popupNavigation = PopupNavigation.Instance;
            _userService = userService;
            AddUserCommand = CommandFactory.Create(OnAddUserClicked);
        }

        public ICommand AddUserCommand { get; }
        public string UserName { get; set; }

        public async Task OnAddUserClicked()
        {
            if (string.IsNullOrEmpty(UserName))
            {
                _messageService.ShortAlert("Please fill in a valid username");
                return;
            }

            try
            {
                await _userService.SaveUserAsync(UserName);
                await _popupNavigation.PopAllAsync();
            }
            catch (System.Exception)
            {
                _messageService.ShortAlert("Please fill in a valid username");
            }
        }
    }
}