using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;

namespace HospitalCalendar.WPF.DataTemplates.Calendar
{
    public class TimeLine : ObservableObject
    {
        public DateTime Day { get; set; }
        public TimeSpan Duration { get; set; }
        public ObservableCollection<TimeLineEvent> Events { get; set; } = new ObservableCollection<TimeLineEvent>();

        public TimeLine(DateTime day, TimeSpan duration)
        {
            Duration = duration + new TimeSpan(1, 0, 0);
            Day = day;
        }
    }
}
