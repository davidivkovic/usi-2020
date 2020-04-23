using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Converters
{
    [ValueConversion(typeof(User), typeof(string))]
    public class UserObjectToRoleStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object upperCase, CultureInfo culture)
        {
            if (value is string s && s == "")
            {
                return "";
            }

            var user = value as User;
            var objectName = user.GetType().Name;

            if (upperCase != null)
            {
                if ((string)upperCase == "true")
                {
                    return objectName.ToUpper();
                }
            }
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objectName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
