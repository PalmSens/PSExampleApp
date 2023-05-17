using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSExampleApp.Core.Helpers
{
    public class TappedGestureAttached
    {
        public static readonly BindableProperty CommandProperty =
            BindableProperty.CreateAttached(
                propertyName: "Command",
                returnType: typeof(ICommand),
                declaringType: typeof(View),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null,
                propertyChanged: OnItemTappedChanged);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.CreateAttached(
                propertyName: "CommandParameter",
                returnType: typeof(object),
                declaringType: typeof(View),
                defaultValue: null,
                defaultBindingMode: BindingMode.OneWay,
                validateValue: null);


        public static object GetCommandParameter(BindableObject bindable)
        {
            return (object)bindable.GetValue(CommandParameterProperty);
        }

        public static void SetCommandParameter(BindableObject bindable, object value)
        {
            bindable.SetValue(CommandParameterProperty, value);
        }

        public static ICommand GetCommand(BindableObject bindable)
        {
            return (ICommand)bindable.GetValue(CommandProperty);
        }

        public static void SetCommand(BindableObject bindable, ICommand value)
        {
            bindable.SetValue(CommandProperty, value);
        }

        public static void OnItemTappedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = bindable as View;

            if (control != null && control.GestureRecognizers != null)
            {
                control.GestureRecognizers?.Clear();
                control.GestureRecognizers?.Add(
                    new TapGestureRecognizer()
                    {
                        Command = new Command((o) =>
                        {

                            var command = GetCommand(control);

                            if (command != null && command.CanExecute(control.GetValue(CommandParameterProperty)))
                                command.Execute(control.GetValue(CommandParameterProperty));
                        })
                    }
                );
            }
        }
    }
}
