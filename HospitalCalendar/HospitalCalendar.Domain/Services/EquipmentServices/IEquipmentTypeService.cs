using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.EquipmentServices
{
    public interface IEquipmentTypeService : IDataService<EquipmentType>
    {
        public Task<EquipmentType> GetByName(string name);
        public Task<bool> EnsureCapacity();
    }
}
