using HospitalCalendar.Domain.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.EquipmentServices
{
    public interface IEquipmentItemService : IDataService<EquipmentItem>
    {
        Task<ICollection<EquipmentItem>> GetAllByType(EquipmentType equipmentType);
        Task<bool> Create(EquipmentType equipmentType, int count);
        Task<ICollection<EquipmentItem>> GetAllWithoutRoom();
    }
}
