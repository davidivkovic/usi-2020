using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class AppointmentScheduleViewModel : ViewModelBase
    {
        private readonly ICalendarEntryService _calendarEntryService;
        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }
        public Calendar Calendar { get; set; }
        public Doctor Doctor { get; set; }
        public Patient CurrentlySelectedPatient { get; set; }
        public CalendarEntry CurrentlySelectedCalendarEntry { get; set; }

        public AppointmentScheduleViewModel(ICalendarEntryService calendarEntryService)
        {
            _calendarEntryService = calendarEntryService;
            MessengerInstance.Register<CurrentUser>(this, message => Doctor = message.User as Doctor);
            Calendar = new Calendar(DateTime.Today);

            MessengerInstance.Register<CalendarEventUnselected>(this, HandleCalendarEventUnselected);
            MessengerInstance.Register<CalendarEventSelected>(this, HandleCalendarEventSelected);
            
            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);
        }

        private static void UiDispatch(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public void Initialize()
        {
            Doctor = DoctorMenuViewModel.Doctor;
            CurrentlySelectedCalendarEntry = null;
            LoadCurrentCalendarWeekForDoctor();
        }

        private void HandleCalendarEventSelected(CalendarEventSelected message)
        {
            CurrentlySelectedCalendarEntry = message.CalendarEntry;
        }

        private void HandleCalendarEventUnselected(CalendarEventUnselected message)
        {
            if(CurrentlySelectedCalendarEntry?.ID == message.CalendarEntry.ID)
                CurrentlySelectedCalendarEntry = null;
        }

        private void LoadCurrentCalendarWeekForDoctor()
        {
            Task.Run(async () =>
            {
                var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
                var entries = (await _calendarEntryService.GetAllByDoctorAndTimeFrame(Doctor, weekStart, weekStart + TimeSpan.FromDays(7))).ToList();
                Calendar.LoadCurrentWeek(entries);
            });
        }

        private void ExecuteLoadPreviousCalendarWeek()
        {
            Task.Run(async () =>
            {
                var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
                var entries = (await _calendarEntryService.GetAllByDoctorAndTimeFrame(Doctor, weekStart - TimeSpan.FromDays(8), weekStart + TimeSpan.FromDays(1))).ToList();
                Calendar.LoadPreviousWeek(entries);
            });
        }

        private void ExecuteLoadNextCalendarWeek()
        {
            Task.Run(async () =>
            {
                var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
                var entries = (await _calendarEntryService.GetAllByDoctorAndTimeFrame(Doctor, weekStart + TimeSpan.FromDays(6), weekStart + TimeSpan.FromDays(15))).ToList();
                Calendar.LoadNextWeek(entries);
            });
        }
    }
}