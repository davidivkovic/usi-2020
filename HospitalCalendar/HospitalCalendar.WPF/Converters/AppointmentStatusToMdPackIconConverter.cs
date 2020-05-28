using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using HospitalCalendar.Domain.Models;
using MaterialDesignThemes.Wpf;

namespace HospitalCalendar.WPF.Converters
{
    public class AppointmentStatusToMdPackIconConverter : IValueConverter
    {
        public object Convert(object appointmentStatus, Type packIconKind, object parameter, CultureInfo culture)
        {
            PackIconKind iconKind;
            switch ((AppointmentStatus)appointmentStatus)
            {
                case AppointmentStatus.Cancelled:
                    iconKind = PackIconKind.MinusCircle;
                    break;
                case AppointmentStatus.Finished:
                    iconKind = PackIconKind.CheckCircle;
                    break;
                case AppointmentStatus.InProgress:
                    iconKind = PackIconKind.Clock;
                    break;
                default:
                    iconKind = PackIconKind.ArrowForwardCircle;
                    break;
            }
            return iconKind;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
