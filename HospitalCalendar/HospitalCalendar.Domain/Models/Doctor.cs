
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Doctor : User {

	public Doctor() {
    }

    public TimeSpan WorkingHoursStart { get; set; }

	public TimeSpan WorkingHoursEnd { get; set; }

	public virtual ICollection<Specialization> Specializations { get; set; }

	public virtual ICollection<DoctorPatient> DoctorsPatients { get; set; }

    
}