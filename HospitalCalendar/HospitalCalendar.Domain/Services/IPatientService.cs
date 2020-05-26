using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services
{
    public interface IPatientService : IDataService<Patient>
    {
        new Task<Patient> Get(Guid ID);
    }
}
