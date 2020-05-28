using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.DataTemplates.Calendar;

namespace HospitalCalendar.WPF.DataTemplates.CalendarEntryTemplate
{
    public class CalendarEntryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SurgeryTemplate { get; set; }
        public DataTemplate AppointmentTemplate { get; set; }
        public DataTemplate RenovationTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var selectedTemplate = AppointmentTemplate;

            var calendarEntryBindableViewModel = item as CalendarEntryBindableViewModel;
            var calendarEntry = calendarEntryBindableViewModel?.CalendarEntry;

            selectedTemplate = calendarEntry switch
            {
                Surgery _ => SurgeryTemplate,
                Appointment _ => AppointmentTemplate,
                Renovation _ => RenovationTemplate,
                _ => selectedTemplate
            };
            return selectedTemplate;
        }
    }
}
