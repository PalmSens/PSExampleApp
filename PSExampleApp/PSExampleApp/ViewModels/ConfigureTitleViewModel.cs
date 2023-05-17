using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resx;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class ConfigureTitleViewModel : BaseAppViewModel
    {
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;
        private string _configuredTitle;

        public ConfigureTitleViewModel(IAppConfigurationService appConfigurationService, IMessageService messageService) : base(appConfigurationService)
        {
            _messageService = messageService;
            _popupNavigation = PopupNavigation.Instance;
            ConfigureTitleCommand = CommandFactory.Create(ConfigureTitle, allowsMultipleExecutions: false);
            CancelCommand = CommandFactory.Create(CancelTitle, allowsMultipleExecutions: false);
            OnPageAppearingCommand = CommandFactory.Create(OnPageAppearing);
        }

        private async Task OnPageAppearing()
        {
            ConfiguredTitle = await _appConfigurationService.GetTitle();
        }

        private async Task CancelTitle()
        {
            await NavigationDispatcher.Pop();
        }
        public string ConfiguredTitle
        {
            get => _configuredTitle;
            set => SetProperty(ref _configuredTitle, value);
        }

        public ICommand ConfigureTitleCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand OnPageAppearingCommand { get; set; }

        private async Task ConfigureTitle()
        {
            await _appConfigurationService.SaveTitle(ConfiguredTitle);
            await NavigationDispatcher.Pop();
            _messageService.LongAlert(AppResources.Alert_TitleConfigured);
        }
    }
}