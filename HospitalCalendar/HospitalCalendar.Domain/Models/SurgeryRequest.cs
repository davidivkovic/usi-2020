
using System;

namespace HospitalCalendar.Domain.Models
{
    public class SurgeryRequest : DomainObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsApproved { get; set; }
        public Patient Patient { get; set; }
        public Doctor Requester { get; set; }
        public Doctor ProposedDoctor { get; set; }
        public Room Room { get; set; }
        public bool IsUrgent { get; set; }
    }
}