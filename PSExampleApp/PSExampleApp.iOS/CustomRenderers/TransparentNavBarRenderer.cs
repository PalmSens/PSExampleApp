using Foundation;
using MaterialComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using PSExampleApp.iOS.CustomRenderers;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(TransparentNavBarRenderer))]
namespace PSExampleApp.iOS.CustomRenderers
{
    public class TransparentNavBarRenderer : NavigationRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (((NavigationPage)Element).CurrentPage is ContentPage)
            {
                NavigationBar.SetBackgroundImage(new UIImage(), UIBarMetrics.Default);
                NavigationBar.ShadowImage = new UIImage();
                NavigationBar.Translucent = true;
            }
        }
    }
}