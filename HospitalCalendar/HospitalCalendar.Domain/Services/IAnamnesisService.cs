using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IAnamnesisService : IDataService<Entry>
    {
        Task<Entry> AddEntry(Appointment appointment, string entry, DateTime timestamp);
        Task<ICollection<Entry>> GetAllByPatient(Patient patient);
    }
}