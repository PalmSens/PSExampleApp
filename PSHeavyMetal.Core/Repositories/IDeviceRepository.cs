using PSHeavyMetal.Common.Models;
using System.Collections.Generic;

namespace PSHeavyMetal.Core.Repositories
{
    public interface IDeviceRepository
    {
        /// <summary>
        /// Gets all known devices
        /// </summary>
        /// <returns></returns>
        List<Device> GetDevices();
    }
}