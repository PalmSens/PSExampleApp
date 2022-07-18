using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PalmSens.Comm;
using PalmSens.Devices;

namespace PalmSens.Core.Simplified
{
    public interface IPlatform
    {
        /// <summary>
        /// Invokes if required.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        bool InvokeIfRequired(Delegate method, params object[] args);

        Task<CommManager> Connect(Device device);

        Task Disconnect(CommManager comm);
    }
}
