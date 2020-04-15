
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Appointment : CalendarEntry
{

	public Appointment() {
	}

    public AppointmentStatus Status { get; set; }

    public Specialization Type { get; set; }

    public Patient Patient { get; set; }

    public Doctor Doctor { get; set; }

    public Room Room { get; set; }

    //public CalendarEntry CalendarEntry { get; set; }

}