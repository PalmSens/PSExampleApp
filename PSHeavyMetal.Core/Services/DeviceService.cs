using PalmSens.Core.Simplified.Android;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly PSCommSimpleAndroid _commService;

        public DeviceService()
        {
        }

        public async Task DetectDevices()
        {
            var blah = await _commService.GetConnectedDevices();
        }

        public List<string> DetectedDevices()
        {
            throw new NotImplementedException();
        }
    }
}