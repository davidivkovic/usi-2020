using System;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services.UserServices
{
    public interface IPatientService : IDataService<Patient>
    {
        new Task<Patient> Get(Guid id);
        Task<Patient> Register(string firstName, string lastName, string userName, string password, string insuranceNumber, Sex sex);
    }
}
