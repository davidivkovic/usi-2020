using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.Domain.Services
{
    interface IRoomService : IDataService<Room>
    {
        Task<Room> GetAllByItem(EquipmentItem equipmentItem);
        Task<Room> GetAllByItems(ICollection<EquipmentItem> equipmentItems);
    }
}
