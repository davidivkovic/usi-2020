using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class AppointmentService : GenericDataService<Appointment>
    {
        public AppointmentService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
