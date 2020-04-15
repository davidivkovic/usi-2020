
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Anamnesis : DomainObject {

	public Anamnesis() {
	}


    public Guid PatientID { get; set; }
    public Patient Patient { get; set; }

    public virtual ICollection<Entry> Entries { get; set; }

}