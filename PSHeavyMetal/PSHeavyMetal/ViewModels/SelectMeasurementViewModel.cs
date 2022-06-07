using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using PSHeavyMetal.Forms.Navigation;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class SelectMeasurementViewModel : BaseViewModel
    {
        private readonly IMeasurementService _measurementService;
        private readonly IPopupNavigation _popupNavigation;
        private readonly IUserService _userService;

        public SelectMeasurementViewModel(IUserService userService, IMeasurementService measurementService)
        {
            OnMeasurementSelectedCommand = CommandFactory.Create(async info => await MeasurementSelected(info as MeasurementInfo));

            _popupNavigation = PopupNavigation.Instance;
            _userService = userService;
            _measurementService = measurementService;
            AddMeasurements();
        }

        public ObservableCollection<MeasurementInfo> AvailableMeasurements { get; } = new ObservableCollection<MeasurementInfo>();

        public ICommand OnMeasurementSelectedCommand { get; }

        private void AddMeasurements()
        {
            foreach (var measurement in _userService.ActiveUser.Measurements)
                AvailableMeasurements.Add(measurement);
        }

        private async Task MeasurementSelected(MeasurementInfo info)
        {
            await _measurementService.LoadMeasurement(info.Id);

            await NavigationDispatcher.Push(NavigationViewType.MeasurementDataView);
            await _popupNavigation.PopAsync();
        }
    }
}