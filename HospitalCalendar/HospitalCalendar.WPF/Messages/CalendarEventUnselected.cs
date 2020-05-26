using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
