using HospitalCalendar.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Runtime.CompilerServices;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface IAppointmentService : IDataService<Appointment>
    {
        Task<ICollection<Appointment>> GetAllByTimeFrame(DateTime start, DateTime end);

        Task<ICollection<Appointment>> GetAllByStatus(AppointmentStatus status);

        Task<ICollection<Appointment>> GetAllPatient(Patient patient);

        Task<ICollection<Appointment>> GetAllDoctor(Doctor doctor);
        Task<ICollection<Appointment>> GetAllByRoom(Room room);

        Task<Appointment> Create(DateTime start, DateTime end, Patient patient, Doctor doctor,Specialization type);

        Task<Appointment> Update(Appointment entity,DateTime start, DateTime end, Patient patient, Doctor doctor, Specialization type, AppointmentStatus status);

    }
}
