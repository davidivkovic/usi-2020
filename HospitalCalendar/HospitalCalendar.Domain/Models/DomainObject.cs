using System;

namespace HospitalCalendar.Domain.Models
{
    public class DomainObject
    {
        public Guid ID { get; set; }
        public bool IsActive { get; set; }
    }
}