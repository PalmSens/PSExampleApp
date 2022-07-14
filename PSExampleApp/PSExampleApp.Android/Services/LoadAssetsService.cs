using PalmSens.Core.Simplified.XF.Application.Services;
using System.IO;

namespace PSExampleApp.Droid.Services
{
    public class LoadAssetsService : ILoadAssetsService
    {
        public StreamReader LoadFile(string filename)
        {
            var assets = Android.App.Application.Context.Assets;

            StreamReader file = new StreamReader(assets.Open(filename));
            return file;
        }
    }
}