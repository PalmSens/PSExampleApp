using PSHeavyMetal.Core.Services;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class StatusBarViewModel : BaseViewModel
    {
        private readonly IDeviceService _deviceService;

        private string _statusText;

        public StatusBarViewModel(IDeviceService deviceService)
        {
            _deviceService = deviceService;
            _statusText = "Searching...";
        }

        public string StatusText
        {
            get => _statusText;
            set => SetProperty(ref _statusText, value);
        }
    }
}