using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    internal class CurrentUser
    {
        public User User { get; set; }

        public CurrentUser(User user)
        {
            User = user;
        }
    }
}