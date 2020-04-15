
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class EquipmentItem : DomainObject
{

	public EquipmentItem() 
    {
	}

	public Room Room { get; set; }
    private EquipmentType EquipmentType { get; set; }

}