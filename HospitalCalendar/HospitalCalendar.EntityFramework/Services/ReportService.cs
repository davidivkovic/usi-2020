using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HospitalCalendar.EntityFramework.Services
{
    public class ReportService:GenericDataService<CalendarEntry>,IReportService
    {
        public ReportService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<bool> GenerateRoomReport(DateTime start, DateTime end) 
        {

            List<Appointment> entrys = new List<Appointment>();
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                entrys =await context.Appointments
                                .Where(ce => ce.IsActive)
                                .ToListAsync();
            }
            


            return false;
        }

        public async Task<bool> GenerateDoctorReport(DateTime start, DateTime end) 
        {

            return false;
        }

        public async Task<bool> GenerateDoctorWorkReport(Doctor doctor)
        {
            return false;
        }

        public async Task<bool> GenerateDoctorPatientReport(Doctor doctor)
        {
            return false;
        }

    }
}
