using System;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
