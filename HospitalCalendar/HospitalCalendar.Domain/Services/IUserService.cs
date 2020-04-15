using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.Domain.Services
{
    public interface IUserService : IDataService<User>
    {
        Task<User> GetByUsername(string username);
    }
}
