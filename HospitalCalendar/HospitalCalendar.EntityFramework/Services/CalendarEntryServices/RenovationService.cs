using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class RenovationService : GenericDataService<Renovation>
    {
        public RenovationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }


    }
}
