using System;

namespace HospitalCalendar.Domain.Models
{
    public class Entry : DomainObject
    {
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public Doctor Doctor { get; set; }
        public Anamnesis Anamnesis { get; set; }
    }
}