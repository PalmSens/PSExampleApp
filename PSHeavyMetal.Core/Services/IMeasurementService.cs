using PalmSens;
using PalmSens.Core.Simplified.Data;
using PSHeavyMetal.Common.Models;
using System.Threading.Tasks;
using static PalmSens.Core.Simplified.PSCommSimple;

namespace PSHeavyMetal.Core.Services
{
    public interface IMeasurementService
    {
        public event SimpleCurveStartReceivingDataHandler DataReceived;

        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public HeavyMetalMeasurement CreateMeasurement(string name, string description);

        public Task<SimpleMeasurement> StartMeasurement(Method method);
    }
}