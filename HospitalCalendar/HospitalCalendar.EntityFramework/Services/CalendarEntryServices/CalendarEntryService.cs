using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class CalendarEntryService : GenericDataService<CalendarEntry>, ICalendarEntryService
    {
        private readonly IRenovationService _renovationService;
        private readonly IAppointmentService _appointmentService;
        private readonly ISurgeryService _surgeryService;

        public CalendarEntryService(HospitalCalendarDbContextFactory contextFactory, IRenovationService renovationService,
            IAppointmentService appointmentService, ISurgeryService surgeryService) : base(contextFactory)
        {
            _renovationService = renovationService;
            _appointmentService = appointmentService;
            _surgeryService = surgeryService;
        }

        public async Task<CalendarEntry> CreateAppointment(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room)
        {
            return await _appointmentService.Create(start, end, doctor, patient, room);
        }

        public async Task<AppointmentRequest> CreateAppointmentRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor,
            DateTime timestamp, Room room)
        {
            return await _appointmentService.CreateAppointmentRequest(start, end, patient, requester, proposedDoctor, timestamp, room);
        }

        public async Task<SurgeryRequest> CreateSurgeryRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor,
            bool isUrgent, DateTime timestamp, Room room)
        {
            return await _surgeryService.CreateSurgeryRequest(start, end, patient, requester, proposedDoctor, isUrgent, timestamp, room);
        }

        public async Task<CalendarEntry> CreateSurgery(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room, bool isUrgent)
        {
            return await _surgeryService.Create(start, end, doctor, patient, room, isUrgent);
        }

        public async Task<ICollection<Appointment>> GetAllByPatient(Patient patient)
        {
            var appointments = new List<Appointment>();

            appointments.AddRange((await _appointmentService.GetAllByPatient(patient)).ToList());
            appointments.AddRange((await _surgeryService.GetAllByPatient(patient)).ToList());

            return appointments;
        }
        public async Task<ICollection<Appointment>> GetAllByPatientAndTimeFrame(Patient patient, DateTime start, DateTime end)
        {
            return (await GetAllByPatient(patient))
                .Where(calendarEntry => (calendarEntry.StartDateTime >= start && calendarEntry.StartDateTime <= end) ||
                                        (calendarEntry.EndDateTime >= start && calendarEntry.EndDateTime <= end) ||
                                        (calendarEntry.StartDateTime >= start && calendarEntry.EndDateTime <= end))
                .ToList();
        }

        public async Task<ICollection<CalendarEntry>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            var calendarEntries = new List<CalendarEntry>();

            calendarEntries.AddRange((await _renovationService.GetAllByTimeFrame(start, end)).ToList());
            calendarEntries.AddRange((await _appointmentService.GetAllByTimeFrame(start, end)).ToList());
            calendarEntries.AddRange((await _surgeryService.GetAllByTimeFrame(start, end)).ToList());

            return calendarEntries;
        }

        public async Task<ICollection<CalendarEntry>> GetAllByRoomAndTimeFrame(Room room, DateTime start, DateTime end)
        {
            var entries = await GetAllByTimeFrame(start, end);
            return entries.Any() ?
                entries.Where(calendarEntry => calendarEntry.Room.Floor == room.Floor && calendarEntry.Room.Number == room.Number).ToList()
                : new List<CalendarEntry>();
        }

        public async Task<ICollection<Appointment>> GetAllByDoctor(Doctor doctor)
        {
            var appointments = new List<Appointment>();

            appointments.AddRange((await _surgeryService.GetAllByDoctor(doctor)).ToList());
            appointments.AddRange((await _appointmentService.GetAllByDoctor(doctor)).ToList());

            return appointments;
        }

        public async Task<ICollection<Appointment>> GetAllByDoctorAndTimeFrame(Doctor doctor, DateTime start, DateTime end)
        {
            return (await GetAllByDoctor(doctor))
                .Where(calendarEntry => (calendarEntry.StartDateTime >= start && calendarEntry.StartDateTime <= end) ||
                            (calendarEntry.EndDateTime >= start && calendarEntry.EndDateTime <= end) ||
                            (calendarEntry.StartDateTime >= start && calendarEntry.EndDateTime <= end))
                .ToList();
        }

        public async Task<bool> CancelCalendarEntry(CalendarEntry calendarEntry)
        {
            if (calendarEntry.Status == AppointmentStatus.Finished || calendarEntry.Status == AppointmentStatus.InProgress)
            {
                return false;
            }

            calendarEntry.Status = AppointmentStatus.Cancelled;
            await Update(calendarEntry);
            return true;
        }
    }
}