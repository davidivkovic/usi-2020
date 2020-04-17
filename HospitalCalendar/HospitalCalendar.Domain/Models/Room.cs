
using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Room : DomainObject
{

	public Room() 
    {
	}

    public int Floor { get; set; }

	public string Number { get; set; }

	public RoomType Type { get; set; }

    public virtual ICollection<EquipmentItem> Equipment { get; set; }

}