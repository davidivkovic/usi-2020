using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IUserService : IDataService<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> GetInactiveByUsername(string username);
        Task<User> Register<T>(string firstName, string lastName, string username, string password) where T : User, new();

        /// <summary>
        /// Update the data of a user in the database.
        /// <para>Throws <see cref="AggregateException"/> if a user with the specified username already exists.</para>
        /// </summary>
        /// <exception cref="AggregateException"></exception>
        Task<User> Update(User user, string firstName, string lastName, string username, string password);
    }
}
