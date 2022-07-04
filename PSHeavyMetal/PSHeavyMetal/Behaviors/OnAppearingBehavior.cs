using System.Windows.Input;
using Xamarin.Forms;

namespace PSHeavyMetal.Forms.Behaviors
{
    public class OnAppearingBehavior : Behavior<ContentPage>
    {
        public static readonly BindableProperty OnAppearingCommandProperty =
            BindableProperty.Create(nameof(OnAppearingCommand), typeof(ICommand), typeof(OnAppearingBehavior));

        public ICommand OnAppearingCommand
        {
            get { return (ICommand)GetValue(OnAppearingCommandProperty); }
            set { SetValue(OnAppearingCommandProperty, value); }
        }

        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);
            this.BindingContext = bindable.BindingContext; // necessary due to design choice in XF, https://github.com/xamarin/Xamarin.Forms/issues/2581
            bindable.Appearing += Bindable_Appearing;
        }

        protected override void OnDetachingFrom(ContentPage bindable)
        {
            bindable.Appearing -= Bindable_Appearing;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Appearing(object sender, System.EventArgs e)
        {
            if (this.OnAppearingCommand == null)
                return;

            this.OnAppearingCommand.Execute(e);
        }
    }
}