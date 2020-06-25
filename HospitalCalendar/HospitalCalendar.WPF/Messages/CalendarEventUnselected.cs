using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class CalendarEventUnselected
    {
        public CalendarEntry CalendarEntry { get; set; }

        public CalendarEventUnselected(CalendarEntry calendarEntry)
        {
            CalendarEntry = calendarEntry;
        }
    }
}
