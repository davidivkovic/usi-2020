using System.Collections.Generic;

namespace HospitalCalendar.Domain.Models
{
    public class Patient : User
    {
        public Sex Sex { get; set; }
        public string InsuranceNumber { get; set; }
        public Anamnesis Anamnesis { get; set; }
        public virtual ICollection<DoctorPatient> DoctorsPatients { get; set; }
    }
}