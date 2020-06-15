using System;
using System.Windows.Data;

namespace HospitalCalendar.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class BoolToInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }
    }
}