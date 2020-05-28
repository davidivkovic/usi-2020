using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services
{
    public class DoctorService : GenericDataService<Doctor>, IDoctorService
    {
        //private readonly HospitalCalendarDbContext _context;
        public DoctorService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
            //_context = contextFactory.CreateDbContext();
        }

        public new async Task<IEnumerable<Doctor>> GetAll()
        {
            //await using var context = ContextFactory.CreateDbContext();
            return await Context.Doctors.Where(d => d.IsActive).Include(d => d.Specializations).ToListAsync();

        }

        public new async Task<Doctor> Get(Guid ID)
        {
            //await using var context = ContextFactory.CreateDbContext();
            return await Context.Doctors.Include(d => d.Specializations).FirstOrDefaultAsync(d => d.IsActive);
        }
    }
}
