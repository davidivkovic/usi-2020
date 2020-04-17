using System.Collections.Generic;

namespace HospitalCalendar.Domain.Models
{
    public class Room : DomainObject
    {
        public int Floor { get; set; }
        public string Number { get; set; }
        public RoomType Type { get; set; }
        public virtual ICollection<EquipmentItem> Equipment { get; set; }
    }
}