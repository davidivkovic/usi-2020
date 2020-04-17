namespace HospitalCalendar.Domain.Models
{
    public class AppointmentChangeRequestNotification : Notification
    {
        public AppointmentChangeRequest AppointmentChangeRequest { get; set; }
    }
}