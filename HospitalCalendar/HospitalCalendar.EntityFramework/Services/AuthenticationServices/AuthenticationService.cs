
using System.Threading.Tasks;
using HospitalCalendar.Domain.Exceptions;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using Microsoft.AspNetCore.Identity;

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

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(storedUser, storedUser.Password, password);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }

            return storedUser;

        }


        public async Task<RegistrationResult> Register<T>(string firstName, string lastName, string username, string password, string confirmPassword) where T : User, new()
        {
            RegistrationResult result = RegistrationResult.Success;

            if (password != confirmPassword)
            {
                result = RegistrationResult.PasswordsDoNotMatch;
            }

            User usernameAccount = await _userService.GetByUsername(username);

            if (usernameAccount != null)
            {
                result = RegistrationResult.UsernameAlreadyExists;
            }

            if (result == RegistrationResult.Success)
            {
                T user = new T()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Username = username,
                    IsActive = true
                };

                string hashedPassword = _passwordHasher.HashPassword(user, password);

                user.Password = hashedPassword;

                await _userService.Create(user);
            }

            return result;
        }
    }
}
