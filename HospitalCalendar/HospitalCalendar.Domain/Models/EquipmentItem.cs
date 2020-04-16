
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
    public EquipmentType EquipmentType { get; set; }


}