using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            return (valueCasted) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility valueBack = (Visibility)value;
            return (valueBack == Visibility.Visible) ? true : false;
        }
    }
}
