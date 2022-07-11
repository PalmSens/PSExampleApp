using MvvmHelpers;
using PSHeavyMetal.Forms.Navigation;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace PSHeavyMetal.Forms.ViewModels
{
    public class ConfigureApplicationViewModel : BaseViewModel
    {
        public ConfigureApplicationViewModel()
        {
            ConfigureAnalyteCommand = CommandFactory.Create(async () => await NavigationDispatcher.Push(NavigationViewType.ConfigureAnalyteView));
        }

        public ICommand ConfigureAnalyteCommand { get; }
    }
}