using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu
{
    public class RenovationMenuViewModel : ViewModelBase
    {
        private readonly ICalendarEntryService _calendarEntryService;
        private readonly IRoomService _roomService;
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;
        private readonly IRenovationService _renovationService;

        public DateTime? RenovationStartDate { get; set; }
        public DateTime? RenovationEndDate { get; set; }
        public DateTime? RenovationStartTime { get; set; }
        public DateTime? RenovationEndTime { get; set; }

        public bool SplittingRoom { get; set; }
        public bool OtherRenovations { get; set; }
        public bool RoomAlreadyInUse { get; set; }
        public bool InvalidTimeFrame { get; set; }

        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }
        public ICommand AddEquipmentItemToRoom { get; set; }
        public ICommand RemoveEquipmentItemFromRoom { get; set; }
        public ICommand LoadRoomsAvailableToJoinTo { get; set; }
        public ICommand ScheduleRenovation { get; set; }

        public ObservableCollection<RoomBindableViewModel> AllRooms { get; set; } = new ObservableCollection<RoomBindableViewModel>();
        public ObservableCollection<RoomBindableViewModel> RoomsAvailableToJoinTo { get; set; } = new ObservableCollection<RoomBindableViewModel>();
        public EquipmentTypeBindableViewModel CurrentlySelectedFreeEquipmentType { get; set; }
        public EquipmentTypeBindableViewModel CurrentlySelectedEquipmentTypeInRoom { get; set; }
        public RoomBindableViewModel RoomToJoinTo { get; set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        public RoomType? NewRoomType { get; set; }

        public ObservableCollection<EquipmentTypeBindableViewModel> FreeEquipmentTypes { get; set; } = new ObservableCollection<EquipmentTypeBindableViewModel>();
        public ObservableCollection<EquipmentTypeBindableViewModel> EquipmentTypesInRoom { get; set; } = new ObservableCollection<EquipmentTypeBindableViewModel>();
        public List<EquipmentType> AddedEquipmentTypes { get; set; } = new List<EquipmentType>();
        public List<EquipmentType> RemovedEquipmentTypes { get; set; } = new List<EquipmentType>();
        public Calendar Calendar { get; set; }
        public RoomBindableViewModel CurrentlySelectedRoom { get; set; }

        // Used by Fody weaver
        private void OnCurrentlySelectedRoomChanged()
        {
            if (CurrentlySelectedRoom is null) return;
            RoomTypes = new ObservableCollection<RoomType>(Enum.GetValues(typeof(RoomType)).Cast<RoomType>().ToList());
            RoomTypes.Remove(RoomTypes.First(rt => rt == CurrentlySelectedRoom.Type));
            NewRoomType = null;
            LoadCurrentCalendarWeekForRoom();
            ExecuteLoadRoomsAvailableToJoinTo();
            AddedEquipmentTypes.Clear();
            RemovedEquipmentTypes.Clear();
            LoadFreeEquipmentTypes();
            LoadRoomEquipmentTypes();
        }

        public RenovationMenuViewModel(ICalendarEntryService calendarEntryService, IRenovationService renovationService, IRoomService roomService,
            IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _calendarEntryService = calendarEntryService;
            _renovationService = renovationService;
            _roomService = roomService;
            _equipmentItemService = equipmentItemService;
            _equipmentTypeService = equipmentTypeService;

            Calendar = new Calendar(DateTime.Today);

            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);
            AddEquipmentItemToRoom = new RelayCommand(ExecuteAddEquipmentItemToRoom);
            RemoveEquipmentItemFromRoom = new RelayCommand(ExecuteRemoveEquipmentItemFromRoom);
            LoadRoomsAvailableToJoinTo = new RelayCommand(ExecuteLoadRoomsAvailableToJoinTo);
            ScheduleRenovation = new RelayCommand(ExecuteScheduleRenovation);
        }

        public async void Initialize()
        {
            RenovationEndDate = RenovationStartDate = null;
            RenovationStartTime = RenovationEndTime = null;
            SplittingRoom = false;
            OtherRenovations = false;
            AddedEquipmentTypes.Clear();
            RemovedEquipmentTypes.Clear();
            EquipmentTypesInRoom.Clear();
            FreeEquipmentTypes.Clear();
            await LoadRooms();
        }

        private async void LoadCurrentCalendarWeekForRoom()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByRoomAndTimeFrame(CurrentlySelectedRoom?.Room, weekStart, weekStart + TimeSpan.FromDays(7))).ToList();
            Calendar.LoadCurrentWeek(entries);
        }

        private async void ExecuteLoadPreviousCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, weekStart - TimeSpan.FromDays(8), weekStart + TimeSpan.FromDays(1))).ToList();
            Calendar.LoadPreviousWeek(entries);
        }

        private async void ExecuteLoadNextCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, weekStart + TimeSpan.FromDays(6), weekStart + TimeSpan.FromDays(15))).ToList();
            Calendar.LoadNextWeek(entries);
        }

        private async Task LoadRooms()
        {
            var rooms = (await _roomService.GetAll()).ToList();
            rooms.ForEach(room => AllRooms.Add(new RoomBindableViewModel(room)));
            CurrentlySelectedRoom = AllRooms.FirstOrDefault();
        }

        private async void LoadFreeEquipmentTypes()
        {
            var equipmentTypes = (await _equipmentTypeService.GetAll()).ToList();
            var tempObsCol = new ObservableCollection<EquipmentTypeBindableViewModel>();

            foreach (var equipmentType in equipmentTypes)
            {
                var amount = await _equipmentItemService.CountFreeByType(equipmentType);
                if (amount > 0)
                {
                    tempObsCol.Add(new EquipmentTypeBindableViewModel(equipmentType, amount));
                }
            }
            FreeEquipmentTypes = tempObsCol;
        }

        private async void LoadRoomEquipmentTypes()
        {
            var equipmentTypes = (await _equipmentTypeService.GetAllByRoom(CurrentlySelectedRoom.Room)).ToList();
            var tempObsCol = new ObservableCollection<EquipmentTypeBindableViewModel>();

            equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));

            foreach (var equipmentType in equipmentTypes)
            {
                var amount = await _equipmentItemService.CountByTypeInRoom(equipmentType, CurrentlySelectedRoom.Room);
                tempObsCol.Add(new EquipmentTypeBindableViewModel(equipmentType, amount));
            }
            EquipmentTypesInRoom = tempObsCol;
        }

        private void ExecuteAddEquipmentItemToRoom()
        {
            var typeToRemove = RemovedEquipmentTypes.FirstOrDefault(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name);
            if (typeToRemove != null)
                RemovedEquipmentTypes.Remove(typeToRemove);

            AddedEquipmentTypes.Add(CurrentlySelectedFreeEquipmentType.EquipmentType);
            FreeEquipmentTypes.First(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name).Amount--;

            // The type is not present in the room
            if (EquipmentTypesInRoom.FirstOrDefault(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name) == null)
            {
                EquipmentTypesInRoom.Insert(0, new EquipmentTypeBindableViewModel(CurrentlySelectedFreeEquipmentType.EquipmentType, 1));
            }
            // The type is present in the room
            else
            {
                EquipmentTypesInRoom.First(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name).Amount++;
            }

            // If the free items have been exhausted for the selected type
            if (FreeEquipmentTypes.First(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name).Amount == 0)
            {
                FreeEquipmentTypes.Remove(FreeEquipmentTypes.First(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name));
                CurrentlySelectedFreeEquipmentType = null;
            }
        }

        private void ExecuteRemoveEquipmentItemFromRoom()
        {
            var typeToRemove = AddedEquipmentTypes.FirstOrDefault(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name);
            if (typeToRemove != null)
                AddedEquipmentTypes.Remove(typeToRemove);

            RemovedEquipmentTypes.Add(CurrentlySelectedEquipmentTypeInRoom.EquipmentType);
            EquipmentTypesInRoom.First(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name).Amount--;

            // The type is not present in the room
            if (FreeEquipmentTypes.FirstOrDefault(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name) == null)
            {
                FreeEquipmentTypes.Insert(0, new EquipmentTypeBindableViewModel(CurrentlySelectedEquipmentTypeInRoom.EquipmentType, 1));
            }
            // The type is present in the room
            else
            {
                FreeEquipmentTypes.First(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name).Amount++;
            }

            // If the free items have been exhausted for the selected type
            if (EquipmentTypesInRoom.First(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name).Amount == 0)
            {
                EquipmentTypesInRoom.Remove(EquipmentTypesInRoom.First(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name));
                CurrentlySelectedEquipmentTypeInRoom = null;
            }
        }

        private async void ExecuteLoadRoomsAvailableToJoinTo()
        {
            var renovationStartPeriod = RenovationStartDate + RenovationStartTime?.TimeOfDay;
            var renovationEndPeriod = RenovationEndDate + RenovationEndTime?.TimeOfDay;

            if (renovationEndPeriod == null || renovationStartPeriod == null)
                return;

            var freeRooms = (await _roomService.GetAllFree(renovationStartPeriod.Value, renovationEndPeriod.Value)).ToList();

            if (freeRooms.FirstOrDefault(r => r.ID == CurrentlySelectedRoom.Room.ID) != null)
            {
                freeRooms.Remove(freeRooms.FirstOrDefault(r => r.ID == CurrentlySelectedRoom.Room.ID));
            }

            RoomsAvailableToJoinTo.Clear();
            freeRooms.ForEach(r => RoomsAvailableToJoinTo.Add(new RoomBindableViewModel(r)));
        }

        private void ExecuteScheduleRenovation()
        {
            Task.Run(async () =>
            {
                RoomAlreadyInUse = InvalidTimeFrame = false;
                var renovationStartPeriod = RenovationStartDate + RenovationStartTime?.TimeOfDay;
                var renovationEndPeriod = RenovationEndDate + RenovationEndTime?.TimeOfDay;

                if (renovationEndPeriod <= renovationStartPeriod || renovationStartPeriod == null || renovationEndPeriod == null)
                {
                    InvalidTimeFrame = true;
                    return;
                }

                // Trying to save some RAM
                if ((await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value)).Count != 0)
                {
                    RoomAlreadyInUse = true;
                    return;
                }

                var addedItems = AddedEquipmentItems();
                var removedItems = RemovedEquipmentItems();

                if (OtherRenovations)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value);
                }
                else if (NewRoomType == null)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value, addedItems, removedItems);
                }
                else if (NewRoomType != null)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, NewRoomType.Value, renovationStartPeriod.Value, renovationEndPeriod.Value, addedItems, removedItems);
                }
                // Create a new renovation which splits the selected room in two
                else if (SplittingRoom)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value, SplittingRoom);
                }
                // Create a new renovation which extends the selected room with another room
                else if (RoomToJoinTo != null)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, RoomToJoinTo.Room, renovationStartPeriod.Value, renovationEndPeriod.Value);
                }

                Calendar.AddEvents((await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value)).ToList());
                RenovationStartDate = RenovationEndDate = RenovationStartTime = RenovationEndTime = null;
                NewRoomType = null;
            });
        }

        private List<EquipmentItem> RemovedEquipmentItems()
        {
            var countByRemovedEquipmentType = RemovedEquipmentTypes
                .GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var removedItems = new List<EquipmentItem>();

            countByRemovedEquipmentType.ForEach(async pair =>
            {
                removedItems.AddRange(
                    (await _equipmentItemService.GetAllByTypeInRoom(pair.Value, CurrentlySelectedRoom.Room))
                    .Take(pair.Count).ToList());
            });
            return removedItems;
        }

        private List<EquipmentItem> AddedEquipmentItems()
        {
            var countByAddedEquipmentType = AddedEquipmentTypes
                .GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var addedItems = new List<EquipmentItem>();

            countByAddedEquipmentType.ForEach(async pair =>
            {
                addedItems.AddRange((await _equipmentItemService.GetAllFreeByType(pair.Value)).Take(pair.Count).ToList());
            });
            return addedItems;
        }
    }
}