using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSSDKXamarinFormsTemplateApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSSDKXamarinFormsTemplateApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //contentView.Content = new ConnectionView();
        }

        internal void SetCurrentView(ContentView value)
        {
            //contentView.Content = value;
        }
    }
}