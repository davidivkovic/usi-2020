namespace HospitalCalendar.Domain.Models
{
    public class Appointment : CalendarEntry
    {
        public AppointmentStatus Status { get; set; }
        public Specialization Type { get; set; }
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }
    }
}