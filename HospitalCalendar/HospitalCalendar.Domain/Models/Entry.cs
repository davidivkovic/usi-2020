
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Entry : DomainObject
{

	public Entry() 
    {
    }

    public DateTime DateCreated { get; set; }

	public string Description { get; set; }

	public Doctor Doctor { get; set; }

    public Anamnesis Anamnesis { get; set; }

}