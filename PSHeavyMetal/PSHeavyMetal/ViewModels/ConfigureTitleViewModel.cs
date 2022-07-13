using MvvmHelpers;
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
        private readonly IPopupNavigation _popupNavigation;

        public ConfigureTitleViewModel(IAppConfigurationService appConfigurationService)
        {
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
        }
    }
}