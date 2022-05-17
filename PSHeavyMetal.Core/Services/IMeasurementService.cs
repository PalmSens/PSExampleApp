using PalmSens;
using PSHeavyMetal.Common.Models;
using System.Threading.Tasks;

namespace PSHeavyMetal.Core.Services
{
    public interface IMeasurementService
    {
        public HeavyMetalMeasurement ActiveMeasurement { get; }

        public HeavyMetalMeasurement CreateMeasurement(string name, string description);

        public void CreatePbMethod();
    }
}