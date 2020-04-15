
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class AppointmentChangeRequest : DomainObject{

	public AppointmentChangeRequest() {
	}


	public DateTime PreviousStartDateTime { get; set; }

	public DateTime PreviousEndDateTime { get; set; }

	public DateTime NewStartDateTime { get; set; }

	public DateTime NewEndDateTime { get; set; }

	public bool IsApproved { get; set; }

	public Appointment Appointment { get; set; }

}