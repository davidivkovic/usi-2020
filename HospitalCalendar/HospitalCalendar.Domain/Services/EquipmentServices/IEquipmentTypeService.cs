using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.EquipmentServices
{
    public interface IEquipmentTypeService : IDataService<EquipmentType>
    {
        public Task<EquipmentType> GetByName(string name);
        public Task<List<EquipmentType>> GetAllByRoom(Room room);
        public Task<EquipmentType> Create(string name, string description, int amount);
        public Task<EquipmentType> Update(EquipmentType entity, string name, string description, int newAmount);
        public new Task<bool> Delete(Guid id);
        public Task<bool> PhysicalDelete(Guid id);
    }
}
