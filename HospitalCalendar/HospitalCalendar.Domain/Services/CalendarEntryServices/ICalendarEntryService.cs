using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface ICalendarEntryService : IDataService<CalendarEntry>
    {
        Task<ICollection<CalendarEntry>> GetAllByTimeFrame(DateTime start, DateTime end);
        Task<ICollection<CalendarEntry>> GetAllByRoomAndTimeFrame(Room room, DateTime start, DateTime end);
        Task<ICollection<Appointment>> GetAllByDoctor(Doctor doctor);
        Task<ICollection<Appointment>> GetAllByDoctorAndTimeFrame(Doctor doctor, DateTime start, DateTime end);
        Task<ICollection<Appointment>> GetAllByPatient(Patient patient);
        Task<CalendarEntry> CreateAppointment(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room);
        Task<CalendarEntry> CreateSurgery(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room, bool isUrgent);
        Task<AppointmentRequest> CreateAppointmentRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor, DateTime timestamp, Room room);
        Task<SurgeryRequest> CreateSurgeryRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor, bool isUrgent, DateTime timestamp, Room room);
        Task<bool> CancelCalendarEntry(CalendarEntry calendarEntry);
    }
}