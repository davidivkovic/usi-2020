using HospitalCalendar.Domain.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.EquipmentServices
{
    public interface IEquipmentItemService : IDataService<EquipmentItem>
    {
        Task<ICollection<EquipmentItem>> GetAllByType(EquipmentType equipmentType);
        Task<ICollection<EquipmentItem>> GetAllFreeByType(EquipmentType equipmentType);
        Task<ICollection<EquipmentItem>> GetAllInUseByType(EquipmentType equipmentType);
        Task<ICollection<EquipmentItem>> GetAllWithoutRoom();
        Task<ICollection<EquipmentItem>> GetAllByRoom(Room room);
        Task<bool> Create(EquipmentType equipmentType, int count);
        Task<bool> Remove(EquipmentType equipmentType, int amount);
        Task<ICollection<EquipmentItem>> GetAllByTypeInRoom(EquipmentType equipmentType, Room room);
    }
}
