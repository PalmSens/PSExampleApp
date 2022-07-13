using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using PSHeavyMetal.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class ConfigureApplicationViewModel : BaseViewModel
    {
        private readonly IAppConfigurationService _appConfigurationService;
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;
        private bool _appConfigured = false;

        public ConfigureApplicationViewModel(IMessageService messageService, IAppConfigurationService appConfigurationService)
        {
            _popupNavigation = PopupNavigation.Instance;
            _appConfigurationService = appConfigurationService;
            _messageService = messageService;
            ConfigureAnalyteCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.ConfigureAnalyteView));
            ConfigureMethodCommand = CommandFactory.Create(ConfigureMethod);
            ConfigureTitleCommand = CommandFactory.Create(ConfigureTitle);
            ConfigureBackgroundCommand = CommandFactory.Create(ConfigureBackground);
        }

        public ICommand ConfigureAnalyteCommand { get; }

        public ICommand ConfigureBackgroundCommand { get; }

        public ICommand ConfigureMethodCommand { get; }

        public ICommand ConfigureTitleCommand { get; }

        private async Task ConfigureBackground()
        {
            var options = new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            };

            try
            {
                var result = await FilePicker.PickAsync(options);

                if (result != null)
                {
                    using var stream = await result.OpenReadAsync();
                    using var memStream = new MemoryStream();

                    await stream.CopyToAsync(memStream);

                    await _appConfigurationService.SaveBackGroundImage(memStream.ToArray());
                    _messageService.LongAlert("Background succesfully saved, please restart the application for the changes to take effect.");
                }
            }
            catch (PermissionException)
            {
                _messageService.LongAlert("Failed to import file. Permissions not set");
            }
            catch (Exception)
            {
                _messageService.LongAlert("Failed importing the image. Please check if the image file has the correct format");
            }
        }

        private async Task ConfigureMethod()
        {
            var customFileType =
                new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                { DevicePlatform.iOS, new[] { "application/octet-stream" } },
                { DevicePlatform.Android, new[] { "application/octet-stream" } },
                });
            var options = new PickOptions
            {
                FileTypes = customFileType,
            };

            try
            {
                var result = await FilePicker.PickAsync(options);

                if (result != null)
                {
                    if (!result.FileName.EndsWith("psmethod"))
                    {
                        _messageService.ShortAlert("Please select a valid psmethod file");
                        return;
                    }

                    using var stream = await result.OpenReadAsync();

                    await _appConfigurationService.SaveConfigurationMethod(stream);
                    _messageService.ShortAlert("Method succesfully saved");
                }
            }
            catch (PermissionException)
            {
                _messageService.LongAlert("Failed to import file. Permissions not set");
            }
            catch (Exception)
            {
                _messageService.LongAlert("Failed importing the method. Please check if the method file has the correct format");
            }
        }

        private async Task ConfigureTitle()
        {
            await _popupNavigation.PushAsync(new ConfigureTitlePopup());
        }
    }
}