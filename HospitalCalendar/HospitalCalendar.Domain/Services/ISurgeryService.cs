using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services
{
    public interface ISurgeryService : IDataService<Surgery>
    {
        Task<ICollection<Surgery>> GetAllByTimeFrame(DateTime start, DateTime end);
        Task<ICollection<Surgery>> GetAllByDoctor(Doctor doctor);
    }
}
