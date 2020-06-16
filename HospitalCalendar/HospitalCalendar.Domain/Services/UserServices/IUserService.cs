using System;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services.UserServices
{
    public interface IUserService : IDataService<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> GetInactiveByUsername(string username);
        Task<User> Register(Type userType, string firstName, string lastName, string username, string password, Sex? sex = null, string insuranceNumber = "");

        /// <summary>
        /// Update the data of a user in the database.
        /// <para>Throws <see cref="AggregateException"/> if a user with the specified username already exists.</para>
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        Task<User> Update(User user, string firstName, string lastName, string username, string password);
    }
}
