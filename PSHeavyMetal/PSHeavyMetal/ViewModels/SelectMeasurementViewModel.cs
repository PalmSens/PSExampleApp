using MvvmHelpers;
using PSHeavyMetal.Common.Models;
using PSHeavyMetal.Core.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class SelectMeasurementViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        public SelectMeasurementViewModel(IUserService userService)
        {
            OnMeasurementSelectedCommand = CommandFactory.Create(MeasurementSelected);

            _userService = userService;
            AddMeasurements();
        }

        public ObservableCollection<MeasurementInfo> AvailableMeasurements { get; } = new ObservableCollection<MeasurementInfo>();

        public ICommand OnMeasurementSelectedCommand { get; }

        private void AddMeasurements()
        {
            foreach (var measurement in _userService.ActiveUser.Measurements)
                AvailableMeasurements.Add(measurement);
        }

        private void MeasurementSelected()
        {
        }
    }
}