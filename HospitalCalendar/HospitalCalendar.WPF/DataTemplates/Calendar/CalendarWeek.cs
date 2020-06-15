using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace HospitalCalendar.WPF.DataTemplates.Calendar
{
    public class CalendarWeek : ObservableObject
    {
        public ObservableCollection<TimeLine> TimeLines { get; set; } = new ObservableCollection<TimeLine>();
        public DateTime WeekStartDateTime { get; set; }
        public bool SpansAcrossTwoMonths { get; set; }
        public bool SpansAcrossTwoYears { get; set; }

        public CalendarWeek(DateTime weekStartDateTime)
        {
            WeekStartDateTime = weekStartDateTime;

            var weekDates = Enumerable.Range(0, 7)
                .Select(offset => weekStartDateTime.AddDays(offset))
                .ToList();

            SpansAcrossTwoMonths = weekDates.First().Month != weekDates.Last().Month;
            SpansAcrossTwoYears = weekDates.First().Year != weekDates.Last().Year;

            weekDates.ForEach(cd =>
            {
                TimeLines.Add(new TimeLine(cd, new TimeSpan(24, 0, 0)));
            });
        }
    }
}
