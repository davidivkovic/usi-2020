namespace HospitalCalendar.Domain.Models
{
    public class Surgery : Appointment
    {
        public bool IsUrgent { get; set; }
    }
}