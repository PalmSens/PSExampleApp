using Android.App;
using Android.Widget;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MessageAndroid))]

namespace PSExampleApp.Droid.Services
{
    public class MessageAndroid : IMessageService
    {
        public void LongAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Long).Show();
        }

        public void ShortAlert(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}