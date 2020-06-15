using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.EntityFramework.Services
{
    public class DoctorService : GenericDataService<Doctor>, IDoctorService
    {
        public DoctorService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public new async Task<IEnumerable<Doctor>> GetAll()
        {
            await using var context = ContextFactory.CreateDbContext();
            var res = await context.Doctors.Where(d => d.IsActive).Include(d => d.Specializations).ToListAsync();
            return res;
        }
        public new async Task<Doctor> Get(Guid id)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Doctors.Include(d => d.Specializations).FirstOrDefaultAsync(d => d.IsActive && d.ID == id);
        }

        public async Task<IEnumerable<Doctor>> GetAllBySpecialization(Specializations specialization)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Doctors
                .Where(d => d.IsActive)
                .Where(d => d.Specializations
                    .Any(s => s.SingleSpecialization == specialization))
                .ToListAsync();

        }

        public async Task<IEnumerable<Doctor>> GetAllOccupied(DateTime start, DateTime end)
        {
            await using var context = ContextFactory.CreateDbContext();
            var occupiedDoctors = await context.Appointments
                .Where(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                            (a.EndDateTime >= start && a.EndDateTime <= end) ||
                            (a.StartDateTime >= start && a.EndDateTime <= end))
                .Where(a => a.IsActive)
                .Select(a => a.Doctor)
                .ToListAsync();

            return occupiedDoctors.GroupBy(r => r.ID)
                .Select(g => g.First())
                .ToList();
        }

        public async Task<IEnumerable<Doctor>> GetAllFree(DateTime start, DateTime end)
        {
            var allDoctors = await GetAll();
            var occupiedDoctors= await GetAllOccupied(start, end);

            return await Task.Run(() =>
            {
                var freeDoctors = allDoctors
                    .Where(r => occupiedDoctors
                        .All(or => or.ID != r.ID))
                    .ToList();
                return freeDoctors;
            });
        }

        public async Task<bool> IsDoctorFreeInTimeSpan(DateTime start, DateTime end, Doctor doctor)
        {
            var freeDoctors = (await GetAllFree(start, end)).ToList();
            return freeDoctors.Any(d => d.ID == doctor.ID);
        }
    }
}