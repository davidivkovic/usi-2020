using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    class NonMatchingPasswordException : Exception
    {
        public NonMatchingPasswordException(string message) : base(message)
        {
        }
    }
}
