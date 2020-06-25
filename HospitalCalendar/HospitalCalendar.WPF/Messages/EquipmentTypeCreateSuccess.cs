using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class EquipmentTypeCreateSuccess
    {
        public EquipmentType EquipmentType { get; set; }
        public int Amount { get; set; }

        public EquipmentTypeCreateSuccess(EquipmentType equipmentType, int amount)
        {
            EquipmentType = equipmentType;
            Amount = amount;
        }
    }
}
