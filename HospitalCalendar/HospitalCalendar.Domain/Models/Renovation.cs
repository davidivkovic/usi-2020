using System.Collections.Generic;

namespace HospitalCalendar.Domain.Models
{
    public class Renovation : CalendarEntry
    {
        public bool Splitting { get; set; }
        public Room RoomToAdd { get; set; }
        public RoomType NewRoomType { get; set; }
        public virtual ICollection<EquipmentItem> RemovedEquipmentItems { get; set; }
        public virtual ICollection<EquipmentItem> AddedEquipmentItems { get; set; }
    }
}