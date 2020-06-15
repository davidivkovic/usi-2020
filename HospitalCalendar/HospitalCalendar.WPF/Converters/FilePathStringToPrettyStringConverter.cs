using System;
using System.Globalization;
using System.Windows.Data;

namespace HospitalCalendar.WPF.Converters
{
    public class FilePathStringToPrettyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filePath = value as string;
            return filePath?.Replace("\\", " > ");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
