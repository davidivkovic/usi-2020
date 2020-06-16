using System;
using System.Collections.Generic;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.NotificationsServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services.NotificationServices
{
    public class NotificationService : GenericDataService<Notification>, INotificationService
    {
        public NotificationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public new async Task<ICollection<Notification>> GetAll()
        {
            var surgeryNotifications = await GetAllSurgeryNotifications();
            var surgeryRequestNotifications = await GetAllSurgeryRequestNotifications();
            var appointmentRequestNotifications = await GetAllAppointmentRequestNotifications();
            var appointmentChangeRequestNotificationsRequest = await GetAllAppointmentChangeRequestNotifications();

            var notifications = new List<Notification>();
            notifications.AddRange(surgeryNotifications);
            notifications.AddRange(surgeryRequestNotifications);
            notifications.AddRange(appointmentRequestNotifications);
            notifications.AddRange(appointmentChangeRequestNotificationsRequest);
            return notifications;
        }
        public async Task<ICollection<SurgeryNotification>> GetAllSurgeryNotifications()
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.SurgeryNotifications
                .Include(surgeryNotification => surgeryNotification.Surgery)
                .ThenInclude(surgery => surgery.Doctor)
                .Include(surgeryNotification => surgeryNotification.Surgery)
                .ThenInclude(surgery => surgery.Patient)
                .Include(surgeryNotification => surgeryNotification.Surgery)
                .ThenInclude(surgery => surgery.Type)
                .Include(surgeryNotification => surgeryNotification.Surgery)
                .ThenInclude(surgery => surgery.Room)
                .ToListAsync();
        }

        public async Task<ICollection<SurgeryRequestNotification>> GetAllSurgeryRequestNotifications()
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.SurgeryRequestNotifications
                .Include(surgeryRequestNotification => surgeryRequestNotification.SurgeryRequest)
                .ThenInclude(surgeryRequest => surgeryRequest.Patient)
                .Include(surgeryRequestNotification => surgeryRequestNotification.SurgeryRequest)
                .ThenInclude(surgeryRequest => surgeryRequest.Room)
                .Include(surgeryRequestNotification => surgeryRequestNotification.SurgeryRequest)
                .ThenInclude(surgeryRequest => surgeryRequest.Requester)
                .Include(surgeryRequestNotification => surgeryRequestNotification.SurgeryRequest)
                .ThenInclude(surgeryRequest => surgeryRequest.ProposedDoctor)
                .ThenInclude(doctor => doctor.Specializations)
                .ToListAsync();
        }

        public async Task<ICollection<AppointmentRequestNotification>> GetAllAppointmentRequestNotifications()
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.AppointmentRequestNotifications
                .Include(appointmentRequestNotification => appointmentRequestNotification.AppointmentRequest)
                .ThenInclude(appointmentRequest => appointmentRequest.Requester)
                .Include(appointmentRequestNotification => appointmentRequestNotification.AppointmentRequest)
                .ThenInclude(appointmentRequest => appointmentRequest.ProposedDoctor)
                .Include(appointmentRequestNotification => appointmentRequestNotification.AppointmentRequest)
                .ThenInclude(appointmentRequest => appointmentRequest.Patient)
                .Include(appointmentRequestNotification => appointmentRequestNotification.AppointmentRequest)
                .ThenInclude(appointmentRequest => appointmentRequest.Room)
                .ToListAsync();
        }
        public async Task<ICollection<AppointmentChangeRequestNotification>> GetAllAppointmentChangeRequestNotifications()
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.AppointmentChangeRequestNotifications
                .Include(appointmentChangeRequestNotification => appointmentChangeRequestNotification.AppointmentChangeRequest)
                .ThenInclude(appointmentChangeRequest => appointmentChangeRequest.Appointment)
                .ThenInclude(appointment => appointment.Doctor)
                .Include(appointmentChangeRequestNotification => appointmentChangeRequestNotification.AppointmentChangeRequest)
                .ThenInclude(appointmentChangeRequest => appointmentChangeRequest.Appointment)
                .ThenInclude(appointment => appointment.Patient)
                .Include(appointmentChangeRequestNotification => appointmentChangeRequestNotification.AppointmentChangeRequest)
                .ThenInclude(appointmentChangeRequest => appointmentChangeRequest.Appointment)
                .ThenInclude(appointment => appointment.Type)
                .Include(appointmentChangeRequestNotification => appointmentChangeRequestNotification.AppointmentChangeRequest)
                .ThenInclude(appointmentChangeRequest => appointmentChangeRequest.Appointment)
                .ThenInclude(appointment => appointment.Room)
                .ToListAsync();
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