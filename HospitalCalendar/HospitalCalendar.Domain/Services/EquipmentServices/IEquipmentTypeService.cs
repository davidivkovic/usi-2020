using HospitalCalendar.Domain.Models;
using System;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.EquipmentServices
{
    public interface IEquipmentTypeService : IDataService<EquipmentType>
    {
        public Task<EquipmentType> GetByName(string name);
        Task<EquipmentType> Create(string name, string description, int amount);
        Task<EquipmentType> Update(EquipmentType entity, string name, string description, int newAmount);
        new Task<bool> Delete(Guid id);
        Task<bool> PhysicalDelete(Guid id);
    }
}
