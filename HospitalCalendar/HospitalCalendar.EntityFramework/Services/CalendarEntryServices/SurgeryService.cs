using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class SurgeryService : GenericDataService<Surgery>, ISurgeryService
    {
        public SurgeryService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<ICollection<Surgery>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();
            return await context.Surgeries
                                .Include(a => a.Room)
                                .Where(a => a.IsActive)
                                .Where(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                                            (a.EndDateTime >= start && a.EndDateTime <= end) ||
                                            (a.StartDateTime >= start && a.EndDateTime <= end))
                                .ToListAsync();
        }

        public async Task<ICollection<Surgery>> GetAllByDoctor(Doctor doctor)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();
            return await context.Surgeries
                                .Include(s => s.Type)
                                .Include(s => s.Patient)
                                .Include(s => s.Doctor)
                                .Include(s => s.Room)
                                .Where(a => a.IsActive)
                                .Where(a => a.Doctor.ID == doctor.ID)
                                .ToListAsync();
        }
    }
}
