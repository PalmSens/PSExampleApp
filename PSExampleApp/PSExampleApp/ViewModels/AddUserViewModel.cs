using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Core.Services;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Diagnostics;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using PSExampleApp.Forms.Resx;

namespace PSExampleApp.Forms.ViewModels
{
    public class AddUserViewModel : BaseAppViewModel
    {
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;

        public AddUserViewModel(IAppConfigurationService appConfigurationService, IUserService userService, IMessageService messageService) :base(appConfigurationService)
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
                _messageService.ShortAlert(AppResources.Alert_ValidUserName);
                return;
            }

            try
            {
                await _userService.SaveUserAsync(UserName);

                var applicationSettings = await _appConfigurationService.GetSettingsAsync();
                applicationSettings.ActiveUserId = _userService.ActiveUser.Id;
                await _appConfigurationService.SaveSettingsAsync(applicationSettings);

                await _popupNavigation.PopAllAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                _messageService.ShortAlert(AppResources.Alert_ValidUserName);
            }
        }
    }
}