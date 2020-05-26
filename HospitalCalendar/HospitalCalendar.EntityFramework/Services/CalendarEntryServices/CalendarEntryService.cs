using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using Microsoft.EntityFrameworkCore;

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
            return entries.Where(ce => ce.Room.Floor == room.Floor && ce.Room.Number == room.Number).ToList();
        }


        public async Task<ICollection<CalendarEntry>> GetAllByDoctor(Doctor doctor)
        {
            var calendarEntries = new List<CalendarEntry>();

            calendarEntries.AddRange((await _surgeryService.GetAllByDoctor(doctor)).ToList());
            calendarEntries.AddRange((await _appointmentService.GetAllByDoctor(doctor)).ToList());

            return calendarEntries;
        }


        public async Task<ICollection<CalendarEntry>> GetAllByDoctorAndTimeFrame(Doctor doctor, DateTime start, DateTime end)
        {
            return (await GetAllByDoctor(doctor))
                .Where(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                            (a.EndDateTime >= start && a.EndDateTime <= end) ||
                            (a.StartDateTime >= start && a.EndDateTime <= end))
                .ToList();
        }
    }
}
