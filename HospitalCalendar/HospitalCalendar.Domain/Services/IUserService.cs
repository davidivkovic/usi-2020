using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;

namespace HospitalCalendar.Domain.Services
{
    public interface IUserService : IDataService<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> Register<T>(string firstName, string lastName, string username, string password, string confirmPassword) where T : User, new();

        /// <summary>
        /// Update the data of a user in the database.
        /// <para>Throws <see cref="AggregateException"/> if a user with the specified username already exists.</para>
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        Task<User> Update(User user, string firstName, string lastName, string username, string password, string confirmPassword);
    }
}
