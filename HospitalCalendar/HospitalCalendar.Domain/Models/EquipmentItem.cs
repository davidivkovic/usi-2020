using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalCalendar.Domain.Models
{
    public class EquipmentItem : DomainObject
    {
        //public Guid EquipmentTypeId { get; set; }
        public Room Room { get; set; }
        //[ForeignKey("EquipmentTypeId")]
        public EquipmentType EquipmentType { get; set; }
    }
}