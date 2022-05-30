using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            InstrumentService.DeviceDiscovered += InstrumentService_DeviceDiscovered;
            InstrumentService.DeviceRemoved += InstrumentService_DeviceRemoved;
            OnInstrumentSelected = MainViewModel.OnInstrumentSelected;
        }

        private void InstrumentService_DeviceDiscovered(object sender, PlatformDevice e)
        {
            AvailableDevices.Add(e);
        }

        private void InstrumentService_DeviceRemoved(object sender, PlatformDevice e)
        {
            var device = AvailableDevices.FirstOrDefault(d => e.DeviceID != null && e.DeviceID == d.DeviceID);

            if (device != null)
            {
                AvailableDevices.Remove(device);
            }
        }
    }
}
