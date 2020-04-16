using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.EquipmentServices
{
    public interface IEquipmentItemService : IDataService<EquipmentItem>
    {
        Task<ICollection<EquipmentItem>> GetAllByType(EquipmentType equipmentType);
        Task<bool> RefreshItems();

    }
}
