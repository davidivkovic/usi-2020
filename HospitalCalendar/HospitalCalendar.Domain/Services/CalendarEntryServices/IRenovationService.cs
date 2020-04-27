using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface IRenovationService : IDataService<Renovation>
    {
        Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
        Task<Renovation> Update(Renovation entity,Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
    }
}
