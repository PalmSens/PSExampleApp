using PSExampleApp.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSExampleApp.Forms.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigureTitleView : ContentPage
	{
		public ConfigureTitleView ()
		{
            BindingContext = App.GetViewModel<ConfigureTitleViewModel>();
            InitializeComponent ();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.UserNameEntry.Focus();
        }
    }
}