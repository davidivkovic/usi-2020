using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    public class EquipmentTypeAlreadyExistsException : Exception
    {
        public EquipmentTypeAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
