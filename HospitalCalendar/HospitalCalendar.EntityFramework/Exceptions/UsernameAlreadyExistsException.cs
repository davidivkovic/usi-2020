using System;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
