using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;

namespace HospitalCalendar.WPF.DataTemplates.Calendar
{
    public class Calendar : ObservableObject
    {
        public DateTime WeekStartDateTime;
        public TimeLine Hours { get; set; }
        public TimeLine CurrentTime { get; set; }
        public CalendarWeek CurrentWeek { get; set; }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        public Calendar(DateTime startWeekDateTime)
        {
            int delta;
            // DayOfWeekStruct specifies Sunday as the first day of the week, this is a hack to use monday as the first day of the week
            if (startWeekDateTime.DayOfWeek == DayOfWeek.Sunday)
            {
                delta = -6;
            }
            else
            {
                delta = DayOfWeek.Monday - startWeekDateTime.DayOfWeek;
            }

            WeekStartDateTime = startWeekDateTime.AddDays(delta);
            CurrentWeek = new CalendarWeek(WeekStartDateTime);

            var dayStart = new TimeSpan(0, 0, 0);
            var dayEnd = new TimeSpan(23, 59, 59);

            Hours = new TimeLine(DateTime.MinValue, dayEnd - dayStart);
            CurrentTime = new TimeLine(DateTime.Today, dayEnd - dayStart);

            Hours.Events = GenerateTimelineHours();

            TrackCurrentTime();
        }

        private static ObservableCollection<TimeLineEvent> GenerateTimelineHours()
        {
            var hourList = Enumerable.Range(new TimeSpan(0, 0, 0).Hours, new TimeSpan(23, 59, 59).Hours + 1).ToList();

            var hourEvents = new ObservableCollection<TimeLineEvent>();

            hourList.ForEach(i =>
            {
                hourEvents.Add(new TimeLineEvent
                {
                    StartDate = DateTime.MinValue + new TimeSpan(i, 0, 0),
                    EndDate = DateTime.MinValue + new TimeSpan(i, 0, 1)
                });
            });
            return hourEvents;
        }

        private void TrackCurrentTime()
        {
            CurrentTime.Events = new ObservableCollection<TimeLineEvent>
            {
                new TimeLineEvent
                {
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.Add(new TimeSpan(0, 0, 1))
                }
            };

            var liveTime = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            liveTime.Tick += (obj, ea) =>
            {
                var currentDateTime = DateTime.Now;

                CurrentTime.Events.First().StartDate = DateTime.Now;
                CurrentTime.Events.First().EndDate = DateTime.Now.Add(new TimeSpan(0, 0, 1));

                var currentDay = CurrentWeek.TimeLines.FirstOrDefault(tl => tl.Day == currentDateTime.Date);

                if (currentDay == null) return;

                var indexOfCurrentDay = CurrentWeek.TimeLines.IndexOf(currentDay);

                foreach (var timeLineEvent in CurrentWeek.TimeLines.ElementAt(indexOfCurrentDay).Events)
                {
                    if (timeLineEvent.CalendarEntry.Status == AppointmentStatus.Cancelled) { }

                    else if (timeLineEvent.CalendarEntry.EndDateTime < DateTime.Now && timeLineEvent.CalendarEntry.StartDateTime < currentDateTime)
                    {
                        CurrentWeek
                            .TimeLines
                            .ElementAt(indexOfCurrentDay)
                            .Events
                            .First(tle => tle.CalendarEntry.StartDateTime == timeLineEvent.CalendarEntry.StartDateTime)
                            .CalendarEntry
                            .Status = AppointmentStatus.Finished;
                    }

                    // Event is in progress
                    else if (timeLineEvent.CalendarEntry.StartDateTime <= currentDateTime && timeLineEvent.CalendarEntry.EndDateTime >= currentDateTime)
                    {
                        CurrentWeek
                            .TimeLines
                            .ElementAt(indexOfCurrentDay)
                            .Events
                            .First(tle =>
                                tle.CalendarEntry.StartDateTime == timeLineEvent.CalendarEntry.StartDateTime)
                            .CalendarEntry
                            .Status = AppointmentStatus.InProgress;
                    }
                }
            };
            liveTime.Start();
        }

        private static List<CalendarEntry> SetCalendarEntriesStatus(List<CalendarEntry> calendarEntries)
        {
            var currentDateTime = DateTime.Now;
            calendarEntries.ForEach(ce =>
            {
                if (ce.Status == AppointmentStatus.Cancelled) { }
                // event finished
                else if (ce.EndDateTime < currentDateTime && ce.StartDateTime < currentDateTime)
                {
                    ce.Status = AppointmentStatus.Finished;
                }

                // Event is in progress
                else if (ce.StartDateTime <= currentDateTime && ce.EndDateTime >= currentDateTime)
                {
                    ce.Status = AppointmentStatus.InProgress;
                }
                // event is over
                else
                {
                    ce.Status = AppointmentStatus.Scheduled;
                }
            });

            return calendarEntries;
        }

        private static List<(DateTime startDateTime, DateTime endDateTime)> SplitCalendarEntryByDays(DateTime endDateTime, DateTime startDateTime)
        {
            TimeSpan delta = endDateTime - startDateTime;

            var dateRanges = Enumerable.Range(0, delta.Days + 2)
                .Select(d =>
                {
                    var dt = startDateTime.AddDays(d);
                    return new
                    {
                        startDate = dt.Date,
                        endOfDate = dt.Date.Add(new TimeSpan(23, 59, 59))
                    };
                })
                .Select(x =>
                (
                    startDate: (x.startDate < startDateTime) ? startDateTime : x.startDate,
                    endDate: (x.endOfDate > endDateTime) ? endDateTime : x.endOfDate
                ))
                .ToList();

            return dateRanges;
        }

        public void AddEvent(CalendarEntry calendarEntry)
        {
            var startDateTime = calendarEntry.StartDateTime;
            var endDateTime = calendarEntry.EndDateTime;

            //await Task.Run(() =>
            //{
                // If a calendar entry spans across multiple days then we split it up by days
                if (startDateTime.Date.CompareTo(endDateTime.Date) != 0)
                {
                    var dateRanges = SplitCalendarEntryByDays(endDateTime, startDateTime);

                    UiDispatch(() =>
                    {
                        dateRanges.ForEach(dateTuple => CurrentWeek.TimeLines
                        .FirstOrDefault(tl => tl.Day.Date.Equals(dateTuple.startDateTime.Date) && tl.Day.Date.Equals(dateTuple.endDateTime.Date))?
                        .Events
                        .Add(new TimeLineEvent
                        {
                            StartDate = dateTuple.startDateTime,
                            EndDate = dateTuple.endDateTime,
                            CalendarEntry = new CalendarEntryBindableViewModel(calendarEntry)
                        }));
                    });
                }
                else
                {
                    var eventToAdd = new TimeLineEvent
                    {
                        StartDate = startDateTime,
                        EndDate = endDateTime,
                        CalendarEntry = new CalendarEntryBindableViewModel(calendarEntry)
                    };
                    UiDispatch(() =>
                    {
                        CurrentWeek.TimeLines
                        .FirstOrDefault(tl => tl.Day.Date.Equals(startDateTime.Date))?
                        .Events
                        .Add(eventToAdd);
                    });
                }
            //});
        }

        public void AddEvents(List<CalendarEntry> calendarEntries)
        {
            //Task.Run(() =>
            //{
                calendarEntries = SetCalendarEntriesStatus(calendarEntries);
                calendarEntries.ForEach(AddEvent);
            //});
        }

        public void LoadCurrentWeek(List<CalendarEntry> calendarEntries)
        {
            CurrentWeek = new CalendarWeek(WeekStartDateTime);
            AddEvents(calendarEntries);
        }

        public void LoadNextWeek(List<CalendarEntry> calendarEntries)
        {
            WeekStartDateTime = WeekStartDateTime.Add(new TimeSpan(7, 0, 0, 0));
            CurrentWeek = new CalendarWeek(WeekStartDateTime);
            AddEvents(calendarEntries);
        }

        public void LoadPreviousWeek(List<CalendarEntry> calendarEntries)
        {
            WeekStartDateTime = WeekStartDateTime.Subtract(new TimeSpan(7, 0, 0, 0));
            CurrentWeek = new CalendarWeek(WeekStartDateTime);
            AddEvents(calendarEntries);
        }
    }
}