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
        Task<User> Login(string username, string password);
    }
}
