using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IRoomService : IDataService<Room>
    {
        Task<ICollection<Room>> GetAllByFloor(int floor);
        Task<Room> GetByFloorAndNumber(int floor, string number);
        Task<ICollection<Room>> GetAllByEquipmentType(EquipmentType equipmentType);
        Task<ICollection<Room>> GetAllByEquipmentTypes(ICollection<EquipmentType> equipmentTypes);
        Task<ICollection<Room>> GetAllOccupied(DateTime start, DateTime end);
        Task<ICollection<Room>> GetAllFree(DateTime start, DateTime end);
        Task<Room> Create(int floor, string number, RoomType type);
        new Task<bool> Delete(Guid id);
    }
}
