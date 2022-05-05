using PSHeavyMetal.Forms.Views;
using System;

using Xamarin.Forms;

namespace PSHeavyMetal.Forms
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AddUserView), typeof(AddUserView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Current.GoToAsync("//LoginPage");
        }
    }
}