using System.Collections.Generic;

namespace HospitalCalendar.Domain.Models
{
    public class Secretary : User
    {
        public virtual ICollection<AppointmentRequestNotification> AppointmentRequestNotifications { get; set; }
        public virtual ICollection<SurgeryNotification> SurgeryNotifications { get; set; }
        public virtual ICollection<AppointmentChangeRequestNotification> AppointmentChangeRequestNotifications { get; set; }
    }
}