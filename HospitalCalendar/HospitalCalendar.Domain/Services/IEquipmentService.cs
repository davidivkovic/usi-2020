using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    interface IEquipmentService : IDataService<EquipmentItem>
    {
        Task<EquipmentItem> GetAllByName(string name);

    }
}
