using System;

namespace HospitalCalendar.Domain.Models
{
    public class Entry : DomainObject
    {
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
        public Appointment Appointment { get; set; }
    }
}