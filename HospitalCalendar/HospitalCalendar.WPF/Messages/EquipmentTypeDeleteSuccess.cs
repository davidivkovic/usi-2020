using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class EquipmentTypeDeleteSuccess
    {
        public EquipmentType EquipmentType { get; set; }

        public EquipmentTypeDeleteSuccess(EquipmentType equipmentType)
        {
            EquipmentType = equipmentType;
        }
    }
}
