using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;

namespace HospitalCalendar.WPF.DataTemplates.Calendar
{
    public class TimeLineEvent : ObservableObject
    {
        [AlsoNotifyFor(nameof(Start))]
        public DateTime StartDate { get; set; }

        [AlsoNotifyFor(nameof(Duration))]
        public DateTime EndDate { get; set; }

        public TimeSpan Start => StartDate.TimeOfDay;
        public TimeSpan Duration => EndDate - StartDate;
        public CalendarEntryBindableViewModel CalendarEntry { get; set; }

        public TimeLineEvent()
        {
            Messenger.Default.Register<DarkModeToggled>(this, message => RaisePropertyChanged(nameof(CalendarEntry)));
        }
    }
}
