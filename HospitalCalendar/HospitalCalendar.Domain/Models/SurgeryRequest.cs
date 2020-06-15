
namespace HospitalCalendar.Domain.Models
{
    public class SurgeryRequest : AppointmentRequest
    {
        public bool IsUrgent { get; set; }
    }
}