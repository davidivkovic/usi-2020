using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.DataTemplates.Calendar;

namespace HospitalCalendar.WPF.DataTemplates.NotificationTemplate
{
    public class NotificationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SurgeryNotificationTemplate { get; set; }
        public DataTemplate SurgeryRequestNotificationTemplate { get; set; }
        public DataTemplate AppointmentRequestNotificationTemplate { get; set; }
        public DataTemplate AppointmentChangeRequestNotificationTemplate { get; set; }


        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedTemplate = SurgeryNotificationTemplate;

            var notification = item as Notification;

            selectedTemplate = notification switch
            {
                SurgeryNotification _ => SurgeryNotificationTemplate,
                SurgeryRequestNotification _ => SurgeryRequestNotificationTemplate,
                AppointmentRequestNotification _ => AppointmentRequestNotificationTemplate,
                AppointmentChangeRequestNotification _ => AppointmentChangeRequestNotificationTemplate,
                _ => selectedTemplate
            };
            return selectedTemplate;
        }
    }
}
