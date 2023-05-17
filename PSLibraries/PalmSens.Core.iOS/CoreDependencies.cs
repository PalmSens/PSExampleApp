using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace PalmSens.Core.iOS
{
    public static class CoreDependencies
    {
        public static void Init()
        {
            PowerManagement.Init(() => { }, () => { }); //Not supported by Apple so do nothing here
        }
    }
}