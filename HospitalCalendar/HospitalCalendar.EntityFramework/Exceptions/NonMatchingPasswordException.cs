using System;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    class NonMatchingPasswordException : Exception
    {
        public NonMatchingPasswordException(string message) : base(message)
        {
        }
    }
}
