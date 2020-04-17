using HospitalCalendar.Domain.Models;
using System;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.CalendarEntryServices
{
    public interface IAppointmentService : IDataService<Appointment>
    {
        Task<Appointment> GetAllByTimeFrame(DateTime start, DateTime end);
    }
}
