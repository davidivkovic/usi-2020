using HospitalCalendar.Domain.Exceptions;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using HospitalCalendar.EntityFramework.Exceptions.HospitalCalendar.Domain.Exceptions;

namespace HospitalCalendar.EntityFramework.Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthenticationService(IUserService userService, IPasswordHasher<User> passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Login(string username, string password)
        {
            var storedUser = await _userService.GetByUsername(username);

            if (storedUser == null)
            {
                throw new InvalidUsernameException(username, password);
            }
            
            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedUser, storedUser.Password, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }
            
            return storedUser;
        }
    }
}
