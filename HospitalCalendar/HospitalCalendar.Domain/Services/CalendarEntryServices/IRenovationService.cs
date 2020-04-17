using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface IRenovationService : IDataService<Renovation>
    { 
        Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems);
    }
}
