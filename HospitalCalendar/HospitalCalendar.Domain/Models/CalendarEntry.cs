using System;

namespace HospitalCalendar.Domain.Models
{
    public class CalendarEntry : DomainObject
    {
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public Room Room { get; set; }
    }
}