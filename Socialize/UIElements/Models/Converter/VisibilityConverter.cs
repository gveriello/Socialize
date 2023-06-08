using System;
using System.Globalization;
using System.Windows.Data;

namespace UnifyMe.Core.Models.Converter
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool valueCasted;
            if (value == null)
                valueCasted = false;
            else
                bool.TryParse(value.ToString(), out valueCasted);
            return (valueCasted) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Windows.Visibility valueBack = (System.Windows.Visibility)value;
            return (valueBack == System.Windows.Visibility.Visible) ? true : false;
        }
    }
}
