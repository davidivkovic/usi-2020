using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class UserRegisterSuccess
    {
        public User User { get; }

        public UserRegisterSuccess(User user)
        {
            User = user;
        }
    }
}