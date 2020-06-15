using HospitalCalendar.Domain.Models;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HospitalCalendar.WPF.Converters
{
    [ValueConversion(typeof(User), typeof(string))]
    public class DomainObjectNameToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object upperCase, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                case string s when s == "":
                    return string.Empty;
            }

            var objectName = value.GetType().Name;

            if (upperCase == null) return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objectName);
            return (string)upperCase == "true" ? objectName.ToUpper() : CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objectName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
