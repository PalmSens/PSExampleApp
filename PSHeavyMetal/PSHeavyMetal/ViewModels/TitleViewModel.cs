using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class TitleViewModel : BaseViewModel
    {
        public IUserService _userService;
        private string _activeUserName;

        public TitleViewModel(IUserService userService)
        {
            _userService = userService;
            ActiveUserName = _userService.ActiveUser?.Name;

            _userService.ActiveUserChanged += _userService_ActiveUserChanged;
        }

        public string ActiveUserName
        {
            get => _activeUserName;
            set => SetProperty(ref _activeUserName, value);
        }

        private void _userService_ActiveUserChanged(object sender, User e)
        {
            ActiveUserName = e.Name;
        }
    }
}