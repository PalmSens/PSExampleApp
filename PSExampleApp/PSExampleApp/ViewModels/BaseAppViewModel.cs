using MvvmHelpers;
using PSExampleApp.Common.Models;
using PSExampleApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSExampleApp.Forms.ViewModels
{
    public class BaseAppViewModel : BaseViewModel
    {
        protected readonly IAppConfigurationService _appConfigurationService;
        private ApplicationSettings _settings;
        public BaseAppViewModel(IAppConfigurationService appConfigurationService)
        {
            _appConfigurationService = appConfigurationService;
            _settings = _appConfigurationService.GetSettings();
        }

        public byte[] BackgroundImage
        {
            get { return _settings.BackgroundImage; }
            private set{}
        }
    }
}
