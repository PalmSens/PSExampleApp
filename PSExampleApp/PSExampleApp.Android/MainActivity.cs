using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using PalmSens.Core.Simplified.Android;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Core.Simplified.XF.Infrastructure.Android.Services;
using Plugin.CurrentActivity;
using PSExampleApp.Droid.Services;
using PSExampleApp.Forms;
using Xamarin.Forms;

namespace PSExampleApp.Droid
{
    [Activity(Label = "PSExampleApp", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public override void OnBackPressed()
        {
            Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Register context required to prevent device going to sleep during measurement
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            Rg.Plugins.Popup.Popup.Init(this);

            //Register platform specific services
            DependencyService.RegisterSingleton(new InstrumentService(new PlatformDeviceManager()));
            DependencyService.RegisterSingleton<IPermissionService>(new PermissionService());
            DependencyService.RegisterSingleton<IMessageService>(new MessageAndroid());

            DependencyService.RegisterSingleton<ILoadAssetsService>(new LoadAssetsService());
            DependencyService.RegisterSingleton<ILoadSavePlatformService>(new SimpleLoadSaveFunctions());

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();

            LoadApplication(new App());
        }
    }
}