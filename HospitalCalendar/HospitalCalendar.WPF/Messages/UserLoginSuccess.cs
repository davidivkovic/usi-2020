using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.ViewModels
{
    public class UserLoginSuccess
    {
        public User User { get; }

        public UserLoginSuccess(User user)
        {
            User = user;
        }
    }
}