using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface IRenovationService : IDataService<Renovation>
    {
        Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
        Task<Renovation> Create(Room room, RoomType newRoomType, DateTime start, DateTime end, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
        Task<Renovation> Create(Room room, DateTime start, DateTime end, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
        Task<Renovation> Create(Room room, DateTime start, DateTime end, bool splitting);
        Task<Renovation> Create(Room room, Room roomToAdd, DateTime start, DateTime end);
        Task<Renovation> Create(Room room, DateTime start, DateTime end);
        Task<Renovation> Update(Renovation renovation, Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
        Task<ICollection<Renovation>> GetAllByTimeFrame(DateTime start, DateTime end);
    }
}
