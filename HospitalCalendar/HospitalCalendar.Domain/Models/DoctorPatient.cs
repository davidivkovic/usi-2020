using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.Domain.Models
{
    public class DoctorPatient
    {
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public Guid PatientId { get; set; }
        public Patient Patient { get; set; }

    }
}
