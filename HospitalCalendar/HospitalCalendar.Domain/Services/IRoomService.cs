using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.Domain.Services
{
    public interface IRoomService : IDataService<Room>
    {
        Task<ICollection<Room>> GetAllByEquipmentType(EquipmentType equipmentType);
        Task<ICollection<Room>> GetAllByEquipmentTypes(ICollection<EquipmentType> equipmentTypes);
    }
}
