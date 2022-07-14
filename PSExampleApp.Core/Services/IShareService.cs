using PSExampleApp.Common.Models;

namespace PSExampleApp.Core.Services
{
    public interface IShareService
    {
        public void CreatePdfFile(HeavyMetalMeasurement measurement, string filePath);
    }
}