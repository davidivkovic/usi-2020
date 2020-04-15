using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services.AuthenticationServices
{
    public enum RegistrationResult
    {
        Success,
        PasswordsDoNotMatch,
        UsernameAlreadyExists
    }

    public interface IAuthenticationService
    {
        Task<RegistrationResult> Register<T>(string firstName, string lastName, string username, string password, string confirmPassword) where T : User, new();
        Task<User> Login(string username, string password);
    }
}
