using System;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    public class NonMatchingPasswordException : Exception
    {
        public NonMatchingPasswordException(string message) : base(message)
        {
        }
    }
}
