using System.IO;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface ILoadSavePlatformService
    {
        public Method LoadMethod(StreamReader streamReader);
    }
}