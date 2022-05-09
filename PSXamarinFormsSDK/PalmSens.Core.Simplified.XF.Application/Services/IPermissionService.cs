using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface IPermissionService
    {
        Task<PermissionStatus> RequestPermission<T>() where T : Permissions.BasePermission, new();
        Task<PermissionStatus> RequestBluetoothPermission();
    }
}