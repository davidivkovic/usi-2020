using System;

namespace HospitalCalendar.Domain.Models
{
    public class AppointmentRequest : DomainObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public Patient Patient { get; set; }
        public Doctor Requester { get; set; }
        public Doctor ProposedDoctor { get; set; }
    }
}