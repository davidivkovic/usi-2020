using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace HospitalCalendar.WPF.Converters
{
    public class DateTimeToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dateOfCalendarDay = ((DateTime)value).DayOfYear;
            var currentDate = DateTime.Today.DayOfYear;

            var dayDelta = Math.Abs(dateOfCalendarDay - currentDate);

            if (dayDelta == 0)
                return 1;

            if (dayDelta >= 5)
                return 0.18;

            var opacity = Math.Abs(dayDelta - 7) / 9.0;
            return opacity;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
