using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface ICalendarEntryService : IDataService<CalendarEntry>
    {
        public Task<ICollection<CalendarEntry>> GetAllByTimeFrame(DateTime start, DateTime end);
        Task<ICollection<CalendarEntry>> GetAllByRoomAndTimeFrame(Room room, DateTime start, DateTime end);
    }
}