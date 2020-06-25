using System;
using System.Windows.Data;

namespace HospitalCalendar.WPF.Converters
{
    public class ComparisonToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(parameter) ?? false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((bool?)value ?? false) ? parameter : Binding.DoNothing;
        }
    }
}