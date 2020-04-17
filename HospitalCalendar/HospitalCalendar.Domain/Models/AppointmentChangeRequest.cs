using System;

namespace HospitalCalendar.Domain.Models
{
    public class AppointmentChangeRequest : DomainObject
    {
        public DateTime PreviousStartDateTime { get; set; }
        public DateTime PreviousEndDateTime { get; set; }
        public DateTime NewStartDateTime { get; set; }
        public DateTime NewEndDateTime { get; set; }
        public bool IsApproved { get; set; }
        public Appointment Appointment { get; set; }
    }
}