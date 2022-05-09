using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using PalmSens.Core.Simplified.XF.Application.Services;
using PalmSens.Core.Simplified.XF.Infrastructure.Android.Services;
using Plugin.CurrentActivity;
using PSSDKXamarinFormsTemplateApp.Droid.Services;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.Droid
{
    [Activity(Label = "PSSDKXamarinFormsTemplateApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            //Register context required to prevent device going to sleep during measurement
            CrossCurrentActivity.Current.Init(this, savedInstanceState);

            //Register platform specific services
            DependencyService.RegisterSingleton(new InstrumentService(new PlatformDeviceManager()));
            DependencyService.RegisterSingleton<IPermissionService>(new PermissionService());

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.FormsMaterial.Init(this, savedInstanceState);

            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}