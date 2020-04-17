using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.EntityFramework.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services
{
    public class UserDataService : GenericDataService<User>, IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserDataService(HospitalCalendarDbContextFactory contextFactory, IPasswordHasher<User> passwordHasher) : base(contextFactory)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetByUsername(string username)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.Users
                                    .Where(u => u.IsActive == true)
                                    .FirstOrDefaultAsync(u => u.Username == username);
            }
        }

        public new async Task<bool> Delete(Guid id)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                var storedUser = await context.Users
                                 .Where(u => u.ID == id)
                                 .FirstOrDefaultAsync(u => u.ID == id);

                storedUser.IsActive = false;

                _ = await Update(storedUser);

                return true;
            }
        }

        public async Task<User> Register<T>(string firstName, string lastName, string username, string password, string confirmPassword) where T : User, new()
        {

            if (password != confirmPassword)
            {
                throw new NonMatchingPasswordException("");
            }

            var usernameAccount = await GetByUsername(username);

            if (usernameAccount != null)
            {
                throw new UsernameAlreadyExistsException(username);
            }

            var newUser = new T()
            {
                FirstName = firstName,
                LastName = lastName,
                Username = username,
                IsActive = true
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, password);

            newUser.Password = hashedPassword;

            _ = await Create(newUser);
            
            return newUser;
        }

        public async Task<User> Update(User user, string firstName, string lastName, string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                throw new NonMatchingPasswordException("");
            }

            var usernameAccount = await GetByUsername(username);

            if (usernameAccount != null)
            {
                throw new UsernameAlreadyExistsException(username);
            }

            user.Username = username;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.IsActive = true;

            var hashedPassword = _passwordHasher.HashPassword(user, password);

            user.Password = hashedPassword;

            return await Update(user);
        }
    }
}
