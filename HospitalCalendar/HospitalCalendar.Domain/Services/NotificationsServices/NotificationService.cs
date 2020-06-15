using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.Domain.Services.NotificationsServices
{
    public interface INotificationService : IDataService<Notification>
    {
        Task<SurgeryNotification> PublishSurgeryNotification(Surgery surgery, DateTime timestamp, string message);
        Task<SurgeryRequestNotification> PublishSurgeryRequestNotification(SurgeryRequest surgeryRequest, DateTime timestamp, string message);
        Task<AppointmentRequestNotification> PublishAppointmentRequestNotification(AppointmentRequest appointmentRequest, DateTime timestamp, string message);
        public Task<AppointmentChangeRequestNotification> PublishAppointmentChangeRequestNotification(
            AppointmentChangeRequest appointmentChangeRequest, DateTime timestamp, string message);
    }
}
