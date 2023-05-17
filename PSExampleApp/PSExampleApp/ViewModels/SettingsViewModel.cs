using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace PSExampleApp.Forms.ViewModels
{
    public class SettingsViewModel : BaseAppViewModel
    {
        private readonly IUserService _userService;
        private bool _isAdmin;
        private Language _language;
        private bool _settingsChanged;

        public SettingsViewModel(IUserService userService, IAppConfigurationService appConfigurationService) : base(appConfigurationService)
        {
            _userService = userService;
            _settingsChanged = false;
            if (_userService?.ActiveUser != null)
            {
                IsAdmin = _userService.ActiveUser.IsAdmin;
            }

            OnPageDisappearingCommand = CommandFactory.Create(OnDisappearing);
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                _userService.ActiveUser.IsAdmin = value;
                _settingsChanged = true;
            }
        }

        public ICommand OnPageDisappearingCommand { get; }

        private void OnDisappearing()
        {
            //Don't do anything if the user setting is changed
            if (!_settingsChanged || _userService?.ActiveUser is null)
                return;

            MessagingCenter.Send<object>(this, "UpdateSettings");
        }
    }
}