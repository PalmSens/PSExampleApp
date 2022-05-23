using System;
using System.Globalization;
using PSSDKXamarinFormsTemplateApp.Pages;
using PSSDKXamarinFormsTemplateApp.Views;
using Xamarin.Forms;

namespace PSSDKXamarinFormsTemplateApp.Converters
{
    public class StringToViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string viewName = (string)value;
            switch (viewName)
            {
                case nameof(ConnectionView):
                    return new ConnectionView();
                case nameof(MeasurementView):
                    return new MeasurementView();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}