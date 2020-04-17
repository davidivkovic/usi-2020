namespace HospitalCalendar.Domain.Models
{
    public class AppointmentRequestNotification : Notification
    {
        public AppointmentRequest AppointmentRequest { get; set; }
    }
}