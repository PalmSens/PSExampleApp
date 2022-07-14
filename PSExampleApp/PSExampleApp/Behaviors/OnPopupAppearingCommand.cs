using Rg.Plugins.Popup.Pages;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSExampleApp.Forms.Behaviors
{
    /// <summary>
    /// On view appearing command uses the layoutchanged command to trigger. This redraws the view and we are able to use it as a hook.
    /// </summary>
    public class OnPopupAppearingBehavior : Behavior<PopupPage>
    {
        public static readonly BindableProperty OnAppearingCommandProperty =
            BindableProperty.Create(nameof(OnAppearingCommand), typeof(ICommand), typeof(OnPopupAppearingBehavior));

        public ICommand OnAppearingCommand
        {
            get { return (ICommand)GetValue(OnAppearingCommandProperty); }
            set { SetValue(OnAppearingCommandProperty, value); }
        }

        protected override void OnAttachedTo(PopupPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.BindingContext = bindable.BindingContext; //necessary due to design choice in XF, https://github.com/xamarin/Xamarin.Forms/issues/2581
            bindable.Appearing += Bindable_Appearing;
        }

        protected override void OnDetachingFrom(PopupPage bindable)
        {
            bindable.Appearing -= Bindable_Appearing;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Appearing(object sender, System.EventArgs e)
        {
            if (OnAppearingCommand == null) return;

            OnAppearingCommand.Execute(e);
        }
    }
}