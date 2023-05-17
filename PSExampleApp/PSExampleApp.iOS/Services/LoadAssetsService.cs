using Foundation;
using PalmSens.Core.Simplified.XF.Application.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UIKit;

namespace PSExampleApp.iOS.Services
{
    public class LoadAssetsService : ILoadAssetsService
    {
        public StreamReader LoadFile(string filename)
        {
            var path = Path.Combine(NSBundle.MainBundle.BundlePath, filename);
            return new StreamReader(path);
        }
    }
}