
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Patient : User {

	public Patient() {
    }

    public Sex Sex { get; set; }

	public string InsuranceNumber { get; set; }

    public Anamnesis Anamnesis { get; set; }

    public virtual ICollection<DoctorPatient> DoctorsPatients { get; set; }

}