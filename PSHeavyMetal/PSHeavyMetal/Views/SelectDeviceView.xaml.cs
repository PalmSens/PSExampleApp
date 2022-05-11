using PSHeavyMetal.Forms.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PSHeavyMetal.Forms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectDeviceView : ContentPage
    {
        public SelectDeviceView()
        {
            BindingContext = App.GetViewModel<SelectDeviceViewModel>();
            InitializeComponent();
        }
    }
}