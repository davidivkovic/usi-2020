
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Renovation : CalendarEntry
{

	public Renovation() {
	}


	public bool Splitting { get; set; }

    public Room RoomToAdd { get; set; }

    public RoomType NewRoomType { get; set; }

    public virtual ICollection<EquipmentItem> RemovedEquipmentItems { get; set; }
    
    public virtual ICollection<EquipmentItem> AddedEquipmentItems { get; set; }

}