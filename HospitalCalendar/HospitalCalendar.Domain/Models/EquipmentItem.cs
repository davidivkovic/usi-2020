using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalCalendar.Domain.Models
{
    public class EquipmentItem : DomainObject
    {
        public Room Room { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }
}