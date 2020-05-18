using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;

namespace HospitalCalendar.WPF.DataTemplates.Calendar
{
    public class CalendarEntryBindableViewModel : ViewModelBase
    {
        [DependsOn(nameof(Status), nameof(EndDateTime), nameof(StartDateTime))]
        public CalendarEntry CalendarEntry { get; set; }

        public AppointmentStatus Status
        {
            get => CalendarEntry.Status;
            set
            {
                CalendarEntry.Status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        public DateTime StartDateTime
        {
            get => CalendarEntry.StartDateTime;
            set
            {
                CalendarEntry.StartDateTime = value;
                RaisePropertyChanged(nameof(StartDateTime));
            }
        }

        public DateTime EndDateTime
        {
            get => CalendarEntry.EndDateTime;
            set
            {
                CalendarEntry.EndDateTime = value;
                RaisePropertyChanged(nameof(EndDateTime));
            }
        }

        public CalendarEntryBindableViewModel(CalendarEntry calendarEntry)
        {
            CalendarEntry = calendarEntry;
            Messenger.Default.Register<DarkModeToggled>(this, message => RaisePropertyChanged(nameof(CalendarEntry)));
        }
    }
}