using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using System.Windows;
using System.Windows.Controls;

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

            CalendarEntry calendarEntry;

            if (item is CalendarEntryBindableViewModel calendarEntryBindableViewModel)
            {
                calendarEntry = calendarEntryBindableViewModel?.CalendarEntry;
            }
            else
            {
                calendarEntry = (CalendarEntry)item;
            }
            
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
