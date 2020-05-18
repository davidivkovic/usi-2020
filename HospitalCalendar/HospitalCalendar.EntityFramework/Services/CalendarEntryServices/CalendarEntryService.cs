using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class CalendarEntryService : GenericDataService<CalendarEntry>, ICalendarEntryService
    {
        private readonly IRenovationService _renovationService;
        private readonly IAppointmentService _appointmentService;
        public CalendarEntryService(HospitalCalendarDbContextFactory contextFactory, IRenovationService renovationService, IAppointmentService appointmentService) : base(contextFactory)
        {
            _renovationService = renovationService;
            _appointmentService = appointmentService;
        }

        public async Task<ICollection<CalendarEntry>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            var calendarEntries = new List<CalendarEntry>();
            
            calendarEntries.AddRange((await _renovationService.GetAllByTimeFrame(start, end)).ToList());
            calendarEntries.AddRange((await _appointmentService.GetAllByTimeFrame(start, end)).ToList());

            return calendarEntries;
        }


        public async Task<ICollection<CalendarEntry>> GetAllByRoomAndTimeFrame(Room room, DateTime start, DateTime end)
        {
            return (await GetAllByTimeFrame(start, end))
                .Where(ce => ce.Room.Floor == room.Floor && ce.Room.Number == room.Number)
                .ToList();
        }
    }
}
