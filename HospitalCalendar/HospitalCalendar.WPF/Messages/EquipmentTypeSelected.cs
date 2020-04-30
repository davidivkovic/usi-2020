using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class EquipmentTypeSelected
    {
        public EquipmentType EquipmentType { get; set; }
        public int Amount { get; set; }

        public EquipmentTypeSelected(EquipmentType equipmentType, int amount)
        {
            EquipmentType = equipmentType;
            Amount = amount;
        }
    }
}
