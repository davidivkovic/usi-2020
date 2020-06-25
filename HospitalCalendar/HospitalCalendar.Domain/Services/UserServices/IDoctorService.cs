using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services.UserServices
{
    public interface IDoctorService : IDataService<Doctor>
    {
        Task<IEnumerable<Doctor>> GetAllBySpecialization(Specializations specialization);
        new Task<IEnumerable<Doctor>> GetAll();
        Task<IEnumerable<Doctor>> GetAllFree(DateTime start, DateTime end);
        Task<IEnumerable<Doctor>> GetAllOccupied(DateTime start, DateTime end);
        new Task<Doctor> Get(Guid id);
        Task<bool> IsDoctorFreeInTimeSpan(DateTime start, DateTime end, Doctor doctor);
    }
}