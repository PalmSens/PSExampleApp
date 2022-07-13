using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.Services;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class ConfigureTitleViewModel : BaseViewModel
    {
        private readonly IAppConfigurationService _appConfigurationService;
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;

        public ConfigureTitleViewModel(IAppConfigurationService appConfigurationService, IMessageService messageService)
        {
            _messageService = messageService;
            _popupNavigation = PopupNavigation.Instance;
            _appConfigurationService = appConfigurationService;
            ConfigureTitleCommand = CommandFactory.Create(ConfigureTitle);
        }

        public string ConfiguredTitle { get; set; }
        public ICommand ConfigureTitleCommand { get; }

        private async Task ConfigureTitle()
        {
            await _appConfigurationService.SaveTitle(ConfiguredTitle);
            await _popupNavigation.PopAllAsync();
            _messageService.LongAlert("Title configured, please restart the application for the changes to take effect.");
        }
    }
}