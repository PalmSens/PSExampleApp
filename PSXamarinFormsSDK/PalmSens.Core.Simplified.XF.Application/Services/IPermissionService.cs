using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface IPermissionService
    {
        public bool CheckBlueToothEnabled();

        public void OpenBluetoothSettings();

        Task<PermissionStatus> RequestBluetoothPermission();

        Task<PermissionStatus> RequestPermission<T>() where T : Permissions.BasePermission, new();
    }
}