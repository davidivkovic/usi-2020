using System;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.NotificationsServices;
using System.Threading.Tasks;

namespace HospitalCalendar.EntityFramework.Services.NotificationServices
{
    public class NotificationService : GenericDataService<Notification>, INotificationService
    {
        public NotificationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {

        }
        public async Task<AppointmentChangeRequestNotification> PublishAppointmentChangeRequestNotification(AppointmentChangeRequest appointmentChangeRequest, DateTime timestamp, string message)
        {
            var notification = new AppointmentChangeRequestNotification();
            await Create(notification);
            notification.IsActive = true;
            notification.AppointmentChangeRequest = appointmentChangeRequest;
            notification.Timestamp = timestamp;
            notification.Status = NotificationStatus.Unread;
            notification.Message = message;

            return await Update(notification) as AppointmentChangeRequestNotification;
        }

        public async Task<AppointmentRequestNotification> PublishAppointmentRequestNotification(AppointmentRequest appointmentRequest, DateTime timestamp, string message)
        {
            var notification = new AppointmentRequestNotification();
            await Create(notification);
            notification.IsActive = true;
            notification.AppointmentRequest = appointmentRequest;
            notification.Timestamp = timestamp;
            notification.Status = NotificationStatus.Unread;
            notification.Message = message;

            return await Update(notification) as AppointmentRequestNotification;
        }
        public async Task<SurgeryRequestNotification> PublishSurgeryRequestNotification(SurgeryRequest surgeryRequest, DateTime timestamp, string message)
        {
            var notification = new SurgeryRequestNotification();

            await Create(notification);
            notification.IsActive = true;
            notification.SurgeryRequest = surgeryRequest;
            notification.Timestamp = timestamp;
            notification.Status = NotificationStatus.Unread;
            notification.Message = message;

            return await Update(notification) as SurgeryRequestNotification;
        }

        public async Task<SurgeryNotification> PublishSurgeryNotification(Surgery surgery, DateTime timestamp, string message)
        {
            var notification = new SurgeryNotification();
            await Create(notification);
            notification.IsActive = true;
            notification.Surgery = surgery;
            notification.Timestamp = timestamp;
            notification.Status = NotificationStatus.Unread;
            notification.Message = message;

            return await Update(notification) as SurgeryNotification;
        }

        public async Task<Notification> MarkAsRead(Notification notification)
        {
            notification.Status = NotificationStatus.Read;
            return await Update(notification);
        }
    }
}