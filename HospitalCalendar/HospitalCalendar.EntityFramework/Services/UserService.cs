using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.EntityFramework.Services
{
    public class UserService : GenericDataService<User>, IUserService
    {
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(HospitalCalendarDbContextFactory contextFactory, IPasswordHasher<User> passwordHasher) : base(contextFactory)
        {
            _passwordHasher = passwordHasher;
        }

        public async Task<User> GetByUsername(string username)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Users
                .Where(u => u.IsActive)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetInactiveByUsername(string username)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Users
                .Where(u => !u.IsActive)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> Register(Type userType, string firstName, string lastName, string username, string password, Sex? sex = null, string insuranceNumber = "")
        {
            var existingUser = await GetByUsername(username);
            var existingUserInactive = await GetInactiveByUsername(username);

            if (existingUser != null || existingUserInactive != null)
            {
                throw new UsernameAlreadyExistsException(username);
            }

            var newUser = (User)Activator.CreateInstance(userType);

            switch (newUser)
            {
                case null:
                    throw new TypeInitializationException("userType was null", null);
                case Patient patient:
                    if (sex != null) patient.Sex = sex.Value;
                    patient.InsuranceNumber = insuranceNumber;
                    newUser = patient;
                    break;
            }

            newUser.FirstName = firstName;
            newUser.LastName = lastName;
            newUser.Username = username;
            newUser.IsActive = true;

            var hashedPassword = _passwordHasher.HashPassword(newUser, password);

            newUser.Password = hashedPassword;

            await Create(newUser);

            return newUser;
        }


        public async Task<User> Update(User user, string newFirstName, string newLastName, string newUsername, string newPassword)
        {
            var existingUser = await GetByUsername(newUsername);

            if (existingUser != null && user.Username != newUsername)
            {
                throw new UsernameAlreadyExistsException(newUsername);
            }

            user.Username = newUsername;
            user.FirstName = newFirstName;
            user.LastName = newLastName;
            user.IsActive = true;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return await Update(user);
            }

            var hashedPassword = _passwordHasher.HashPassword(user, newPassword);
            user.Password = hashedPassword;

            return await Update(user);
        }

        public new async Task<bool> Delete(Guid id)
        {
            var existingUser = await Get(id);
            existingUser.IsActive = false;

            await Update(existingUser);

            return true;
        }
    }
}
