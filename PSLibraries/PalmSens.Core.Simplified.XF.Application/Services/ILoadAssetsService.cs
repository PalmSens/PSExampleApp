using System.IO;

namespace PalmSens.Core.Simplified.XF.Application.Services
{
    public interface ILoadAssetsService
    {
        public StreamReader LoadFile(string filename);
    }
}