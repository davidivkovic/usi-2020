using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    class UserUpdateRequest
    {
        public User User { get; }

        public UserUpdateRequest(User user)
        {
            User = user;
        }
    }
}
