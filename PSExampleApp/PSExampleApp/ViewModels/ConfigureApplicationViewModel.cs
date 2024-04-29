using MvvmHelpers;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Core.Services;
using PSExampleApp.Forms.Navigation;
using PSExampleApp.Forms.Resx;
using PSExampleApp.Forms.Views;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PSExampleApp.Forms.ViewModels
{
    public class ConfigureApplicationViewModel : BaseAppViewModel
    {
        private readonly IMessageService _messageService;
        private readonly IPopupNavigation _popupNavigation;
        private bool _appConfigured = false;

        public ConfigureApplicationViewModel(IMessageService messageService, IAppConfigurationService appConfigurationService) : base(appConfigurationService)
        {
            _popupNavigation = PopupNavigation.Instance;     
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
                    _messageService.LongAlert(AppResources.Alert_BackgroundSaved);
                }
            }
            catch (PermissionException)
            {
                _messageService.LongAlert(AppResources.Alert_FailedImport);
            }
            catch (Exception)
            {
                _messageService.LongAlert(AppResources.Alert_FailedImageImport);
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
                PickerTitle = AppResources.Picker_SelectMethodFile,
                FileTypes = customFileType,
            };

            try
            {
                FileResult result = null;
                if (Device.RuntimePlatform == Device.iOS)
                {
                    result = await FilePicker.PickAsync();
                }
                else if (Device.RuntimePlatform == Device.Android)
                {
                    result = await FilePicker.PickAsync(options);
                }
                
                if (result != null)
                {
                    if (!result.FileName.EndsWith("psmethod"))
                    {
                        _messageService.ShortAlert(AppResources.Alert_SelectValidMethodFile);
                        return;
                    }

                    await using var stream = await result.OpenReadAsync();

                    await _appConfigurationService.SaveConfigurationMethod(stream);
                    var loadedMethod = await _appConfigurationService.LoadConfigurationMethod();
                    
                    // Post Checks
                    if (loadedMethod == null)
                    {
                        _messageService.LongAlert(AppResources.Alert_FailedImportMethod);
                        return;
                    }
                    
                    if (loadedMethod.nScans > 1)
                    {
                        _messageService.LongAlert(AppResources.Alert_MethodIncompatibleNumberOfScans);
                        return;
                    }
                    
                    _messageService.ShortAlert(AppResources.Alert_MethodSaved);
                }
            }
            catch (PermissionException)
            {
                _messageService.LongAlert(AppResources.Alert_FailedImport);
            }
            catch (Exception)
            {
                _messageService.LongAlert(AppResources.Alert_FailedImportMethod);
            }
        }

        private async Task ConfigureTitle()
        {
            await NavigationDispatcher.Push(NavigationViewType.ConfigureTitleView);
        }
    }
}