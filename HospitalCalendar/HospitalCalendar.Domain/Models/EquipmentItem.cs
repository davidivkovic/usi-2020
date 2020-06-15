namespace HospitalCalendar.Domain.Models
{
    public class EquipmentItem : DomainObject
    {
        public Room Room { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}