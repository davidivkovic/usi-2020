using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IEquipmentService : IDataService<EquipmentItem>
    {
        Task<List<EquipmentItem>> GetAllByName(string name);

    }
}
