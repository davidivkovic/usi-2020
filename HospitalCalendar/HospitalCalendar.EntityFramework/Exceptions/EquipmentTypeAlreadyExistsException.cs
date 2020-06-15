using System;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    public class EquipmentTypeAlreadyExistsException : Exception
    {
        public EquipmentTypeAlreadyExistsException(string message) : base(message)
        {
        }
    }
}
