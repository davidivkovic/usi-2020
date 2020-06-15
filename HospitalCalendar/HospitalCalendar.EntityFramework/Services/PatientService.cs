using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.EntityFramework.Services
{
    public class PatientService : GenericDataService<Patient>, IPatientService
    {
        private readonly IUserService _userService;
        public PatientService(HospitalCalendarDbContextFactory contextFactory, IUserService userService) : base(contextFactory)
        {
            _userService = userService;
        }

        //public new async Task<Patient> Get(Guid id)
        //{
        //    await using var context = ContextFactory.CreateDbContext();
        //    return await context.Patients
        //        //.Include(p => p.Anamnesis)
        //            //.ThenInclude(a => a.Entries)
        //        //.Include(a => a.Anamnesis)
        //        .FirstOrDefaultAsync(p => p.ID == id);
        //}

        public async Task<Patient> Register(string firstName, string lastName, string userName, string password, string insuranceNumber, Sex sex)
        {
            return await _userService.Register(typeof(Patient), firstName, lastName, userName, password) as Patient;
        }
    }
}