using System.Windows.Input;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.Behaviors
{
    /// <summary>
    /// On view appearing command uses the layoutchanged command to trigger. This redraws the view and we are able to use it as a hook.
    /// </summary>
    public class OnViewAppearingBehavior : Behavior<ContentView>
    {
        public static readonly BindableProperty OnAppearingCommandProperty =
            BindableProperty.Create(nameof(OnAppearingCommand), typeof(ICommand), typeof(OnAppearingBehavior));

        public ICommand OnAppearingCommand
        {
            get { return (ICommand)GetValue(OnAppearingCommandProperty); }
            set { SetValue(OnAppearingCommandProperty, value); }
        }

        protected override void OnAttachedTo(ContentView bindable)
        {
            base.OnAttachedTo(bindable);
            this.BindingContext = bindable.BindingContext; //necessary due to design choice in XF, https://github.com/xamarin/Xamarin.Forms/issues/2581
            bindable.LayoutChanged += Bindable_Appearing;
        }

        protected override void OnDetachingFrom(ContentView bindable)
        {
            bindable.LayoutChanged -= Bindable_Appearing;
            base.OnDetachingFrom(bindable);
        }

        private void Bindable_Appearing(object sender, System.EventArgs e)
        {
            if (OnAppearingCommand == null) return;

            OnAppearingCommand.Execute(e);
        }
    }
}