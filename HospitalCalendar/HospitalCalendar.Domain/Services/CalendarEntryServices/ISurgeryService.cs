using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface ISurgeryService : IDataService<Surgery>
    {
        Task<ICollection<Surgery>> GetAllByTimeFrame(DateTime start, DateTime end);
        Task<ICollection<Surgery>> GetAllByDoctor(Doctor doctor);
        Task<Surgery> Create(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room, bool isUrgent);
        Task<ICollection<Surgery>> GetAllByPatient(Patient patient);
        Task<AppointmentRequest> CreateSurgeryRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor, bool isUrgent, DateTime timestamp, Room room);
    }
}
    