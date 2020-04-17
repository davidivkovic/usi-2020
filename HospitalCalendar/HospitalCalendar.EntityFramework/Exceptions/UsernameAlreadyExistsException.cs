using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
