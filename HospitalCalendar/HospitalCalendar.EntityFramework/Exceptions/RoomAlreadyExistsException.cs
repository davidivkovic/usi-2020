using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalCalendar.EntityFramework.Exceptions
{
    public class RoomAlreadyExistsException : Exception
    {
        public int Floor { get; set; }
        public string Number { get; set; }

        public RoomAlreadyExistsException(int floor, string number)
        {
            Floor = floor;
            Number = number;
        }
    }
}
