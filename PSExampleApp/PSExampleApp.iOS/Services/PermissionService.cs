using CoreBluetooth;
using CoreFoundation;
using Foundation;
using PalmSens.Core.Simplified.XF.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;

namespace PSExampleApp.iOS.Services
{
    public class PermissionService : IPermissionService
    {
        private bool HasBluetoothPermission()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                return CBCentralManager.Authorization == CBManagerAuthorization.AllowedAlways;
            }
            else
            {
                return true;
            }
        }

        public async Task<PermissionStatus> RequestBluetoothPermission()
        {
            if (!HasBluetoothPermission())
            {
                var tcs = new TaskCompletionSource<bool>();
                var myDelegate = new PermissionCBCentralManager(tcs);
                var centralManger = new CBCentralManager(myDelegate, DispatchQueue.MainQueue, new CBCentralInitOptions() { ShowPowerAlert = false });
                await tcs.Task;

                return HasBluetoothPermission() ? PermissionStatus.Granted : PermissionStatus.Denied;
            }
            return PermissionStatus.Granted;
        }

        public Task<PermissionStatus> RequestPermission<T>() where T : Permissions.BasePermission, new()
        {
            return Task.FromResult(PermissionStatus.Granted);
        }

        public bool CheckBlueToothEnabled()
        {
            // Note: iOS doesn't provide a way to check if Bluetooth is enabled or not
            return true;
        }

        public void OpenBluetoothSettings()
        {
            // Note: iOS doesn't provide a way to directly open Bluetooth settings
            //throw new NotImplementedException();
        }
    }

    public class PermissionCBCentralManager : CBCentralManagerDelegate
    {
        TaskCompletionSource<bool> _tcs = null;

        public PermissionCBCentralManager(TaskCompletionSource<bool> tcs)
        {
            _tcs = tcs;
        }

        public override void UpdatedState(CBCentralManager central)
        {
            _tcs.SetResult(true);
        }
    }
}