using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Messages
{
    public class UserUpdateRequest
    {
        public User User { get; }

        public UserUpdateRequest(User user)
        {
            User = user;
        }
    }
}
