using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using PalmSens.Core.Simplified.XF.Application.Models;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.ViewModels
{
    internal class ConnectionViewModel : ViewViewModel
    {
        public ICommand OnInstrumentSelected { get; }

        public ObservableCollection<PlatformDevice> AvailableDevices { get; } =
            new ObservableCollection<PlatformDevice>();

        public ConnectionViewModel()
        {
            InstrumentService.DeviceDiscovered += _instrumentService_DeviceDiscovered;
            OnInstrumentSelected = MainViewModel.OnInstrumentSelected;
        }

        private void _instrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            AvailableDevices.Add(e);
        }
    }
}
