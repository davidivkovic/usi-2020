
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using HospitalCalendar.Domain.Models;

public class Renovation : CalendarEntry
{

	public Renovation() {
	}

    public bool ChangingRoomType { get; set; }

	public bool MovingEquipment { get; set; }

	public bool ChangingLayout { get; set; }

	public RoomType NewRoomType { get; set; }

    [NotMapped]
    public virtual ICollection<EquipmentItem> RemovedEquipmentItems { get; set; }

    [NotMapped]
    public virtual ICollection<EquipmentItem> AddedEquipmentItems { get; set; }


}