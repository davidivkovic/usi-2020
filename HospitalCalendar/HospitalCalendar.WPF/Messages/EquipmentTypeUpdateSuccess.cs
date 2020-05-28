using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class EquipmentTypeUpdateSuccess
    {
        public EquipmentType EquipmentType { get; set; }
        public int NewAmount { get; set; }

        public EquipmentTypeUpdateSuccess(EquipmentType equipmentType, int newAmount)
        {
            EquipmentType = equipmentType;
            NewAmount = newAmount;
        }
    }
}
