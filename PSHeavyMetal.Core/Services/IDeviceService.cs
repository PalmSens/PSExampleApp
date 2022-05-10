using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IDeviceService
    {
        public Task DetectDevices();

        public List<string> DetectedDevices();
    }
}