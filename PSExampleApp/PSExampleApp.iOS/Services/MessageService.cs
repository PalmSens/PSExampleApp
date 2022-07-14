using Foundation;
using PalmSens.Core.Simplified.XF.Application.Services;
using PSExampleApp.iOS.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(MessageIOS))]

namespace PSExampleApp.iOS.Services
{
    public class MessageIOS : IMessageService
    {
        private const double LONG_DELAY = 3.5;
        private const double SHORT_DELAY = 2.0;

        private UIAlertController alert;
        private NSTimer alertDelay;

        public void LongAlert(string message)
        {
            ShowAlert(message, LONG_DELAY);
        }

        public void ShortAlert(string message)
        {
            ShowAlert(message, SHORT_DELAY);
        }

        private void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }

        private void ShowAlert(string message, double seconds)
        {
            alertDelay = NSTimer.CreateScheduledTimer(seconds, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }
    }
}