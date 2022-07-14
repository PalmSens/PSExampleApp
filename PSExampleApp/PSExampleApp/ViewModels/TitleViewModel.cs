using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class TitleViewModel : BaseViewModel
    {
        public readonly IAppConfigurationService _appConfigurationService;
        public readonly IUserService _userService;
        private string _activeUserName;

        public TitleViewModel(IUserService userService, IAppConfigurationService appConfiguration)
        {
            _appConfigurationService = appConfiguration;
            _userService = userService;
            ActiveUserName = _userService.ActiveUser?.Name;

            _userService.ActiveUserChanged += _userService_ActiveUserChanged;

            OnViewAppearingCommand = CommandFactory.Create(OnViewAppearing);
        }

        public string ActiveUserName
        {
            get => _activeUserName;
            set => SetProperty(ref _activeUserName, value);
        }

        public string ApplicationTitle => _appConfigurationService.CurrentApplicationSettings.Title;

        public ICommand OnViewAppearingCommand { get; }

        private void _userService_ActiveUserChanged(object sender, User e)
        {
            ActiveUserName = e.Name;
        }

        private void OnViewAppearing()
        {
            OnPropertyChanged(nameof(ApplicationTitle));
        }
    }
}