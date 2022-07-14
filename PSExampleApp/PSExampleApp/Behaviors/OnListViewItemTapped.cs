using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace PSExampleApp.Forms.Behaviors
{
    public class OnListViewItemTapped : Behavior<ListView>
    {
        public static readonly BindableProperty OnItemTappedCommandProperty =
            BindableProperty.Create(nameof(OnItemTappedCommand), typeof(ICommand), typeof(OnListViewItemTapped));

        public ICommand OnItemTappedCommand
        {
            get { return (ICommand)GetValue(OnItemTappedCommandProperty); }
            set { SetValue(OnItemTappedCommandProperty, value); }
        }

        protected override void OnAttachedTo(ListView listView)
        {
            base.OnAttachedTo(listView);
            BindingContext = listView.BindingContext;
            listView.BindingContextChanged += ListView_BindingContextChanged;
            listView.ItemTapped += ListView_ItemTapped;
        }

        protected override void OnDetachingFrom(ListView listView)
        {
            listView.ItemTapped -= ListView_ItemTapped;
            listView.BindingContextChanged -= ListView_BindingContextChanged;
            base.OnDetachingFrom(listView);
        }

        private void ListView_BindingContextChanged(object sender, EventArgs e)
        {
            if (sender is ListView listView)
            {
                BindingContext = listView.BindingContext;
            }
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (OnItemTappedCommand == null) return;

            OnItemTappedCommand.Execute(e.Item);
        }
    }
}