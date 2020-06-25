
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services
{
    public class AnamnesisService : GenericDataService<Entry>, IAnamnesisService
    {
        public AnamnesisService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<Entry> AddEntry(Appointment appointment, string description, DateTime timestamp)
        {
            await using var context = ContextFactory.CreateDbContext();
            var entry = new Entry();
            await Create(entry);
            entry.Appointment = appointment;
            entry.Patient = appointment.Patient;
            entry.DateCreated = timestamp;
            entry.Description = description;
            entry.Doctor = appointment.Doctor;
            entry.IsActive = true;

            return await Update(entry);
        }

        public async Task<ICollection<Entry>> GetAllByPatient(Patient patient)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Entries
                .Include(entry => entry.Appointment)
                    .ThenInclude(appointment => appointment.Type)
                .Include(entry => entry.Doctor)
                .Include(entry => entry.Patient)
                .Where(entry => entry.Patient.ID == patient.ID)
                .ToListAsync();
        }
    }
}
