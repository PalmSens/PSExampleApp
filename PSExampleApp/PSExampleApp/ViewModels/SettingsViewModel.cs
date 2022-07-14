using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSExampleApp.Forms.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly IUserService _userService;
        private bool _isAdmin;
        private Language _language;
        private bool _settingsChanged;

        public SettingsViewModel(IUserService userService)
        {
            _userService = userService;
            Language = _userService.ActiveUser.Language;
            IsAdmin = _userService.ActiveUser.IsAdmin;
            _settingsChanged = false;

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

        public Language Language
        {
            get => _language;
            set
            {
                _language = value;
                _settingsChanged = true;
                ChangeLanguage(value);
            }
        }

        public ICommand OnPageDisappearingCommand { get; }

        private void ChangeLanguage(Language language)
        {
            var code = _userService.GetActiveUserLanguageCode(language);
            LocalizationResourceManager.Current.CurrentCulture = code == null ? CultureInfo.CurrentCulture : new CultureInfo(code);
        }

        private async Task OnDisappearing()
        {
            //Don't do anything if the user setting is changed
            if (!_settingsChanged)
                return;

            await _userService.UpdateUserSettings(Language);
        }
    }
}