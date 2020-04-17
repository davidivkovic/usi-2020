using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class AppointmentService : GenericDataService<Appointment>
    {
        public AppointmentService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
