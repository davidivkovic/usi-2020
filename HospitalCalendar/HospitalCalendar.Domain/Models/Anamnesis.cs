using System;
using System.Collections.Generic;

namespace HospitalCalendar.Domain.Models
{
    public class Anamnesis : DomainObject
    {
        public Guid PatientID { get; set; }
        public Patient Patient { get; set; }
        public virtual ICollection<Entry> Entries { get; set; }
    }
}