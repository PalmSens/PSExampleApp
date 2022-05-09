using System.Windows.Input;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.Behaviors
{
    public class OnDisappearingBehavior : Behavior<ContentPage>
    {
        public static readonly BindableProperty OnDisappearingCommandProperty =
            BindableProperty.Create(nameof(OnDisappearingCommand), typeof(ICommand), typeof(OnAppearingBehavior));

        public ICommand OnDisappearingCommand
        {
            get { return (ICommand)GetValue(OnDisappearingCommandProperty); }
            set { SetValue(OnDisappearingCommandProperty, value); }
        }

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.BindingContext = bindable.BindingContext; //necessary due to design choice in XF, https://github.com/xamarin/Xamarin.Forms/issues/2581
            bindable.Disappearing += Bindable_Disappearing;
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            bindable.Appearing -= Bindable_Disappearing;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Disappearing(object sender, System.EventArgs e)
        {
            if(OnDisappearingCommand == null) return;

            OnDisappearingCommand.Execute(e);
        }
    }
}
