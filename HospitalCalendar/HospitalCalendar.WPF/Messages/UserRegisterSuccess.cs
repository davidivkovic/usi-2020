using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.ViewModels
{
    internal class UserRegisterSuccess
    {
        public User User { get; }

        public UserRegisterSuccess(User user)
        {
            User = user;
        }
    }
}