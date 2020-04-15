
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class EquipmentType : DomainObject
{

	public EquipmentType() 
    {
	}

    public string Name { get; set; }

	public int TotalAmount { get; set; }

	public int FreeAmount { get; set; }

	public int InUseAmount { get; set; }


}