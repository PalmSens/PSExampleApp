using Rg.Plugins.Popup.Pages;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSExampleApp.Forms.Behaviors
{
    internal class OnPopupDisappearingBehavior : Behavior<PopupPage>
    {
        public static readonly BindableProperty OnDisappearingCommandProperty =
            BindableProperty.Create(nameof(OnDisappearingCommand), typeof(ICommand), typeof(OnPopupDisappearingBehavior));

        public ICommand OnDisappearingCommand
        {
            get { return (ICommand)GetValue(OnDisappearingCommandProperty); }
            set { SetValue(OnDisappearingCommandProperty, value); }
        }

        protected override void OnAttachedTo(PopupPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.BindingContext = bindable.BindingContext; //necessary due to design choice in XF, https://github.com/xamarin/Xamarin.Forms/issues/2581
            bindable.Disappearing += Bindable_Disappearing;
        }

        protected override void OnDetachingFrom(PopupPage bindable)
        {
            bindable.Disappearing -= Bindable_Disappearing;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Disappearing(object sender, System.EventArgs e)
        {
            if (OnDisappearingCommand == null) return;

            OnDisappearingCommand.Execute(e);
        }
    }
}