using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services
{
    public class PatientService : GenericDataService<Patient>, IPatientService
    {
        public PatientService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
            //_context = contextFactory.CreateDbContext();
        }

        public new async Task<Patient> Get(Guid id)
        {
            return await Context.Patients
                .Include(p => p.Anamnesis)
                    .ThenInclude(a => a.Entries)
                .FirstOrDefaultAsync(p => p.ID == id);
        }
    }
}
