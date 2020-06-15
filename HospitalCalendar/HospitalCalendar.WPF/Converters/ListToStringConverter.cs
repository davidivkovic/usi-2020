using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HospitalCalendar.WPF.Converters
{
    public class ListToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return value != null ? string.Join(" ", ((ICollection<Specialization>)value).Select(s => s.SingleSpecialization).ToList()) : "";
            return value != null ? ((Specialization)value).SingleSpecialization.ToString() : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
