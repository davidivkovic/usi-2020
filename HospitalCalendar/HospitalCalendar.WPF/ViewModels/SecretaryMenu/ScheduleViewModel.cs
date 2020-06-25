using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.SecretaryMenu
{
    public class ScheduleViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly ICalendarEntryService _calendarEntryService;
        public List<Room> AvailableRooms { get; set; }
        public Room SelectedRoom { get; set; }
        public Calendar Calendar { get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }
        public ICommand CancelCalendarEntry { get; set; }
        public CalendarEntry SelectedCalendarEntry { get; set; }

        public ScheduleViewModel(IRoomService roomService, ICalendarEntryService calendarEntryService)
        {
            _calendarEntryService = calendarEntryService;
            _roomService = roomService;

            MessengerInstance.Register<CalendarEventUnselected>(this, HandleCalendarEventUnselected);
            MessengerInstance.Register<CalendarEventSelected>(this, HandleCalendarEventSelected);

            AvailableRooms = new List<Room>();
            Calendar = new Calendar(DateTime.Today);
            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);
            CancelCalendarEntry = new RelayCommand(ExecuteCancelCalendarEntry);
        }

        // Used by Fody Weaver
        private async void OnSelectedRoomChanged()
        {
            if (SelectedRoom == null) return;
            await LoadCurrentCalendarWeekForRoom();
        }

        public async void Initialize()
        {
            await LoadRooms();
        }

        private async Task LoadRooms()
        {
            var availableRooms = (await _roomService.GetAll()).Where(r => r.Type == RoomType.CheckUp || r.Type == RoomType.Surgery).ToList();
            AvailableRooms = new List<Room>(availableRooms);
            SelectedRoom = availableRooms.FirstOrDefault();
        }

        private async Task LoadCurrentCalendarWeekForRoom()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByRoomAndTimeFrame(SelectedRoom, weekStart, weekStart + TimeSpan.FromDays(7))).ToList();
            Calendar.LoadCurrentWeek(entries);
        }

        private async void ExecuteLoadPreviousCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByRoomAndTimeFrame(SelectedRoom, weekStart - TimeSpan.FromDays(8), weekStart + TimeSpan.FromDays(1))).ToList();
            Calendar.LoadPreviousWeek(entries);
        }

        private async void ExecuteLoadNextCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByRoomAndTimeFrame(SelectedRoom, weekStart + TimeSpan.FromDays(6), weekStart + TimeSpan.FromDays(15))).ToList();
            Calendar.LoadNextWeek(entries);
        }
        private void HandleCalendarEventSelected(CalendarEventSelected message)
        {
            SelectedCalendarEntry = message.CalendarEntry;
        }

        private void HandleCalendarEventUnselected(CalendarEventUnselected message)
        {
            //if (SelectedCalendarEntry?.ID == message.CalendarEntry.ID)
                //SelectedCalendarEntry = null;
        }
        private async void ExecuteCancelCalendarEntry()
        {
            SelectedCalendarEntry.Status = AppointmentStatus.Cancelled;
            await _calendarEntryService.Update(SelectedCalendarEntry);
            await LoadCurrentCalendarWeekForRoom();
        }
    }
}
