using System;
using System.Collections.Generic;

namespace HospitalCalendar.Domain.Models
{
    public class Doctor : User
    {
        public TimeSpan WorkingHoursStart { get; set; }
        public TimeSpan WorkingHoursEnd { get; set; }
        public virtual ICollection<Specialization> Specializations { get; set; }
        public virtual ICollection<DoctorPatient> DoctorsPatients { get; set; }
    }
}