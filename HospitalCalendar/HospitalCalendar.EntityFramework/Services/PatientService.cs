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
        public async Task<Patient> Register(string firstName, string lastName, string userName, string password, Sex sex, string insuranceNumber)
        {
            return await _userService.Register(typeof(Patient), firstName, lastName, userName, password, sex, insuranceNumber) as Patient;
        }
    }
}