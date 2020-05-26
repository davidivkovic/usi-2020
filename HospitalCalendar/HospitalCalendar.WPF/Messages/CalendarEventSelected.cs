using System;
using System.Collections.Generic;
using System.Text;
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
