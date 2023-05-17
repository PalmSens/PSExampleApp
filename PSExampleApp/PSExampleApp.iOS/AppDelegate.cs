using Foundation;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Core.Simplified.XF.Infrastructure.iOS.Services;
using ProgressRingControl.Forms.Plugin.iOS;
using PSExampleApp.Forms;
using PSExampleApp.iOS.Services;
using UIKit;
using Xamarin.Forms;

namespace PSExampleApp.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Rg.Plugins.Popup.Popup.Init();
            global::Xamarin.Forms.Forms.Init();
            global::Xamarin.Forms.FormsMaterial.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            OxyPlot.Xamarin.Forms.Platform.iOS.PlotViewRenderer.Init();
            //Register platform specific services
            DependencyService.RegisterSingleton(new InstrumentService(new PlatformDeviceManager()));
            DependencyService.RegisterSingleton<IPermissionService>(new PermissionService());
            DependencyService.RegisterSingleton<IMessageService>(new MessageIOS());

            DependencyService.RegisterSingleton<ILoadAssetsService>(new LoadAssetsService());
            DependencyService.RegisterSingleton<ILoadSavePlatformService>(new SimpleLoadSaveFunctions());
            Xamarin.IQKeyboardManager.SharedManager.Enable = true;
            LoadApplication(new App());
            ProgressRingRenderer.Init();

            return base.FinishedLaunching(app, options);
        }
    }
}