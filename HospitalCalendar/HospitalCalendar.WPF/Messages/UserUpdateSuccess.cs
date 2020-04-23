using System;
using System.Collections.Generic;
using System.Text;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class UserUpdateSuccess
    {
        public User User { get; }

        public UserUpdateSuccess(User user)
        {
            User = user;
        }
    }
}
