
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class CalendarEntry : DomainObject
{

	public CalendarEntry() 
    {
	}

    public DateTime StartDateTime { get; set; }

	public DateTime EndDateTime { get; set; }

}