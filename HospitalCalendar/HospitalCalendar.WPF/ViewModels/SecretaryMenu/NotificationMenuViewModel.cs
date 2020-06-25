using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.NotificationsServices;

namespace HospitalCalendar.WPF.ViewModels.SecretaryMenu
{
    public class NotificationMenuViewModel : ViewModelBase
    {
        private readonly INotificationService _notificationService;
        private readonly ICalendarEntryService _calendarEntryService;
        public ICommand ResolveNotification { get; set; }
        public ObservableCollection<Notification> Notifications { get; set; }
        public Notification SelectedNotification { get; set; }
        public bool NotificationIsUnresolved { get; set; }

        private void OnSelectedNotificationChanged()
        {
            NotificationIsUnresolved = SelectedNotification?.Status != NotificationStatus.Resolved;
        }
        public NotificationMenuViewModel(INotificationService notificationService, ICalendarEntryService calendarEntryService)
        {
            _notificationService = notificationService;
            _calendarEntryService = calendarEntryService;
            Notifications = new ObservableCollection<Notification>();
            ResolveNotification = new RelayCommand(ExecuteResolveNotification);
        }

        private async void ExecuteResolveNotification()
        {
            switch (SelectedNotification)
            {
                case SurgeryRequestNotification notification:
                    await _calendarEntryService.CreateSurgery(notification.SurgeryRequest.StartDate,
                        notification.SurgeryRequest.EndDate, notification.SurgeryRequest.ProposedDoctor,
                        notification.SurgeryRequest.Patient, notification.SurgeryRequest.Room, 
                        notification.SurgeryRequest.IsUrgent);
                    break;
                case AppointmentRequestNotification notification:
                    await _calendarEntryService.CreateAppointment(notification.AppointmentRequest.StartDate,
                        notification.AppointmentRequest.EndDate, notification.AppointmentRequest.ProposedDoctor,
                        notification.AppointmentRequest.Patient, notification.AppointmentRequest.Room);
                    break;
            }
            SelectedNotification.Status = NotificationStatus.Resolved;
            await _notificationService.Update(SelectedNotification);
            NotificationIsUnresolved = false;
            await LoadNotifications();
        }

        public async void Initialize()
        {
            await LoadNotifications();
        }

        public async Task LoadNotifications()
        {
            Notifications.Clear();
            var notifications = (await _notificationService.GetAll()).ToList();
            notifications.Reverse();
            notifications.ForEach(notification => Notifications.Add(notification));
        }
    }
}
