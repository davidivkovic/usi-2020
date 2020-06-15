using System;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    namespace HospitalCalendar.Domain.Exceptions
    {
        public class InvalidUsernameException : Exception
        {
            public string Username { get; }
            public string Password { get; }

            public InvalidUsernameException(string username, string password)
            {
                Username = username;
                Password = password;
            }

            public InvalidUsernameException(string message, string username, string password) : base(message)
            {
                Username = username;
                Password = password;
            }

            public InvalidUsernameException(string message, Exception innerException, string username, string password) : base(message, innerException)
            {
                Username = username;
                Password = password;
            }
        }
    }
}
