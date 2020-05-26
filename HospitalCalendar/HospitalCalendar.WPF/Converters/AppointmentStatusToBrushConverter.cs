using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using HospitalCalendar.Domain.Models;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace HospitalCalendar.WPF.Converters
{
    public class AppointmentStatusToBrushConverter : IValueConverter
    {
        public object Convert(object appointmentStatus, Type targetType, object convertTextToBrush, System.Globalization.CultureInfo culture)
        {   
            var paletteHelper = new PaletteHelper();
            //Retrieve the app's existing theme
            var theme = paletteHelper.GetTheme();

            Brush textBrush, backgroundBrush;

            switch ((AppointmentStatus)appointmentStatus)
            {
                case AppointmentStatus.Finished:
                { 
                    if (theme.Background == Theme.Dark.MaterialDesignBackground)
                    {
                        textBrush = Brushes.White;
                        backgroundBrush = new SolidColorBrush(SwatchHelper.Lookup[MaterialDesignColor.Grey900]);
                    }
                    else
                    {
                        textBrush = Brushes.Black;
                        backgroundBrush = new SolidColorBrush(SwatchHelper.Lookup[MaterialDesignColor.Grey100]);
                    }
                    break;
                }
                case AppointmentStatus.InProgress:
                {
                    textBrush = Brushes.White;
                    backgroundBrush = new SolidColorBrush(theme.PrimaryMid.Color);
                    break;
                }
                case AppointmentStatus.Scheduled:
                {
                    textBrush = Brushes.Black;
                    backgroundBrush = new SolidColorBrush(SwatchHelper.Lookup[MaterialDesignColor.Amber500]);
                    break;
                }
                default:
                {
                    textBrush = Brushes.White;
                    backgroundBrush = new SolidColorBrush(SwatchHelper.Lookup[MaterialDesignColor.PinkA400]);
                    break;
                }
            }

            if (convertTextToBrush == null) return backgroundBrush;

            return (string)convertTextToBrush == "true" ? textBrush : backgroundBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
