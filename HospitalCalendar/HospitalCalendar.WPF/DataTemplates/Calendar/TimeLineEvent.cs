using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;
using System;
using System.Windows.Input;

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
        public ICommand EventSelected { get; set; }
        public ICommand EventUnselected { get; set; }
        public bool IsWorkingHoursStartOrEnd { get; set; }

        public TimeLineEvent()
        {
            EventSelected = new RelayCommand(ExecuteEventSelected);
            EventUnselected = new RelayCommand(ExecuteEventUnselected);
            Messenger.Default.Register<DarkModeToggled>(this, message => RaisePropertyChanged(nameof(CalendarEntry)));
        }

        private void ExecuteEventSelected()
        {
            Messenger.Default.Send(new CalendarEventSelected(CalendarEntry.CalendarEntry));
        }
        private void ExecuteEventUnselected()
        {
            Messenger.Default.Send(new CalendarEventUnselected(CalendarEntry.CalendarEntry));
        }
    }
}
