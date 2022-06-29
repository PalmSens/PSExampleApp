using PSHeavyMetal.Common.Models;

namespace PSHeavyMetal.Core.Services
{
    public interface IShareService
    {
        public void CreatePdfFile(HeavyMetalMeasurement measurement, string filePath);
    }
}