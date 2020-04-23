using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class RenovationService : GenericDataService<Renovation>
    {
        public RenovationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
