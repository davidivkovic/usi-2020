using System;

namespace HospitalCalendar.Domain.Models
{
    public class Notification : DomainObject
    {
        public string Message { get; set; }
        public NotificationStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
    }
}