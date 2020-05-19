using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.EntityFramework.Services
{
    class DoctorService : GenericDataService<Doctor>, IDoctorService
    {
        public DoctorService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
