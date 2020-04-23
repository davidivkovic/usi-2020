using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Converters
{
    [ValueConversion(typeof(Enum), typeof(IEnumerable<Enum>))]
    public class RoomTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object upperCase, CultureInfo culture)
        {
            if (value is string s && s == "")
            {
                return "";
            }

            var enumElement = value as Enum;
            
            if (upperCase != null)
            {
                if ((string)upperCase == "true")
                {
                    return enumElement?.ToString().ToUpper();
                }
            }
            return enumElement?.ToString();
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
