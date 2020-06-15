using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface IAppointmentService : IDataService<Appointment>
    {
        Task<ICollection<Appointment>> GetAllByTimeFrame(DateTime start, DateTime end);
        Task<ICollection<Appointment>> GetAllByStatus(AppointmentStatus status);
        Task<ICollection<Appointment>> GetAllByPatient(Patient patient);
        Task<ICollection<Appointment>> GetAllByDoctor(Doctor doctor);
        Task<ICollection<Appointment>> GetAllByRoom(Room room);
        Task<Appointment> Create(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room);
        Task<Appointment> Update(Appointment appointment, DateTime start, DateTime end, Patient patient, Doctor doctor, Specialization type, AppointmentStatus status);
        Task<AppointmentRequest> CreateAppointmentRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor, DateTime timestamp, Room room);
    }
}
