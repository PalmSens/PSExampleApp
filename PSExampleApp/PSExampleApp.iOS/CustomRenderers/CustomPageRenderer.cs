using Foundation;
using MathNet.Numerics.Distributions;
using PSExampleApp.Forms.Views;
using PSExampleApp.iOS.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Page), typeof(CustomPageRenderer))]
namespace PSExampleApp.iOS.CustomRenderers
{
    public class CustomPageRenderer : PageRenderer
    {

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(false);

            if (Element is ContentPage page && !(Element is HomeView))
            {
                if (NavigationController != null)
                {
                    var root = NavigationController.TopViewController;
                    var backButton = new UIBarButtonItem("<", UIBarButtonItemStyle.Plain, (sender, args) =>
                    {
                        page.Navigation.PopAsync(false);
                    });
                    UITextAttributes attributes = new UITextAttributes()
                    {
                        Font = UIFont.BoldSystemFontOfSize(26)
                    };
                    backButton.SetTitleTextAttributes(attributes, UIControlState.Normal);
                    root.NavigationItem.SetLeftBarButtonItem(backButton, false);
                }

            }
        }
    }
}