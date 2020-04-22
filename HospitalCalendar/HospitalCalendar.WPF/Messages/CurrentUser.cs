using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.ViewModels
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