using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class CalendarEventSelected
    {
        public CalendarEntry CalendarEntry { get; set; }

        public CalendarEventSelected(CalendarEntry calendarEntry)
        {
            CalendarEntry = calendarEntry;
        }
    }
}
