using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu
{
    public class RenovationMenuViewModel : ViewModelBase
    {
        private readonly ICalendarEntryService _calendarEntryService;
        private readonly IRoomService _roomService;
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;
        private readonly IRenovationService _renovationService;
        private RoomBindableViewModel _currentlySelectedRoom;

        public DateTime? RenovationStartDate { get; set; } = null;
        public DateTime? RenovationEndDate { get; set; } = null;
        public DateTime? RenovationStartTime { get; set; } = null;
        public DateTime? RenovationEndTime { get; set; } = null;

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

        public ObservableCollection<EquipmentTypeBindableViewModel> FreeEquipmentTypes = new ObservableCollection<EquipmentTypeBindableViewModel>();
        public ObservableCollection<EquipmentTypeBindableViewModel> EquipmentTypesInRoom { get; set; } = new ObservableCollection<EquipmentTypeBindableViewModel>();
        public List<EquipmentType> AddedEquipmentTypes { get; set; } = new List<EquipmentType>();
        public List<EquipmentType> RemovedEquipmentTypes { get; set; } = new List<EquipmentType>();
        public Calendar Calendar { get; set; }

        public RoomBindableViewModel CurrentlySelectedRoom
        {
            get => _currentlySelectedRoom;
            set
            {
                if (_currentlySelectedRoom == value)
                    return;
                _currentlySelectedRoom = value;
                RaisePropertyChanged(nameof(CurrentlySelectedRoom));
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
        }

        public RenovationMenuViewModel(ICalendarEntryService calendarEntryService, IRenovationService renovationService, IRoomService roomService,
            IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _calendarEntryService = calendarEntryService;
            _renovationService = renovationService;
            _roomService = roomService;
            _equipmentItemService = equipmentItemService;
            _equipmentTypeService = equipmentTypeService;

            var room = Task.Run(async () => await roomService.GetAll()).GetAwaiter().GetResult().ToList().First();
            //Task.Run(async () => await userService.Register<Doctor>("David", "Ivkovic", "nuts", "deez")).GetAwaiter().GetResult();
            //var doc = Task.Run(async () => await userService.GetAll()).GetAwaiter().GetResult().Where(u=> u.GetType() == typeof(Doctor)).ToList().First();

            LoadRooms();


            /*
            List<CalendarEntry> entries = new List<CalendarEntry>
            {
                new Appointment{ StartDateTime = DateTime.Today + new TimeSpan(22, 0, 0), EndDateTime = DateTime.Today + new TimeSpan(22, 30, 0)},
                new Surgery{ StartDateTime = DateTime.Today + new TimeSpan(12, 0, 0), EndDateTime = DateTime.Today + new TimeSpan(12, 30, 0), IsUrgent = true, Room = room},
                new Appointment { StartDateTime = DateTime.Today + new TimeSpan(23, 0, 0), EndDateTime = DateTime.Today + new TimeSpan(24, 0, 0)},
                new Renovation { StartDateTime = new DateTime(2020, 5, 12, 16, 0, 0), EndDateTime = new DateTime(2020, 5, 12, 19, 30, 0), Room = room},
                new Renovation { StartDateTime = new DateTime(2020, 5, 13, 8, 0, 0), EndDateTime = new DateTime(2020, 5, 13, 10, 30, 0), Room = room},
                new Appointment { StartDateTime = new DateTime(2020, 5, 5, 0, 0, 0), EndDateTime = new DateTime(2020, 5, 5, 1, 0, 0)},
                new Appointment { StartDateTime = new DateTime(2020, 5, 7, 12, 0, 0), EndDateTime = new DateTime(2020, 5, 7, 12, 30, 0)},
                //new Appointment { StartDateTime = new DateTime(2020, 5, 10, 12, 0, 0), EndDateTime = new DateTime(2020, 5, 11, 12, 30, 0), 
                //    Status = AppointmentStatus.Cancelled,
                //    Type = new Specialization{SingleSpecialization = Specializations.Dermatology},
                //    Doctor = (Doctor)doc},
                    new Appointment { StartDateTime = new DateTime(2020, 5, 4, 6, 0, 0), EndDateTime = new DateTime(2020, 5, 4, 6, 30, 0)},
                new Appointment { StartDateTime = DateTime.Now.Subtract(new TimeSpan(0, 5, 0)) , EndDateTime = DateTime.Now.Add(new TimeSpan(0, 25, 0))}

            };*/
            Calendar = new Calendar(DateTime.Today);

            //DateTime weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            //var entries = new List<CalendarEntry>();
            //Task.Run(async () =>
            //{
            //    if (CurrentlySelectedRoom != null)
            //        entries = (await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, weekStart, weekStart + TimeSpan.FromDays(7))).ToList();
            //});
            //Calendar.AddEvents(entries);

            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);
            AddEquipmentItemToRoom = new RelayCommand(ExecuteAddEquipmentItemToRoom);
            RemoveEquipmentItemFromRoom = new RelayCommand(ExecuteRemoveEquipmentItemFromRoom);
            LoadRoomsAvailableToJoinTo = new RelayCommand(ExecuteLoadRoomsAvailableToJoinTo);
            ScheduleRenovation = new RelayCommand(ExecuteScheduleRenovation);
        }

        private void LoadCurrentCalendarWeekForRoom()
        {
            Task.Run(async() =>
            {
                var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
                var entries = (await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom?.Room, weekStart, weekStart + TimeSpan.FromDays(7))).ToList();

                Calendar.LoadCurrentWeek(entries);
            });
        }

        private void ExecuteLoadPreviousCalendarWeek()
        {
            Task.Run(async () =>
            {
                var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
                var entries = (await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, weekStart - TimeSpan.FromDays(8), weekStart + TimeSpan.FromDays(1))).ToList();
                Calendar.LoadPreviousWeek(entries);
            });
        }

        private void ExecuteLoadNextCalendarWeek()
        {
            Task.Run(async () =>
            {
                var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
                var entries = (await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, weekStart + TimeSpan.FromDays(6), weekStart + TimeSpan.FromDays(15))).ToList();
                Calendar.LoadNextWeek(entries);
            });
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        private void LoadRooms()
        {
            Task.Run(async () =>
            {
                var rooms = (await _roomService.GetAll()).ToList();
                rooms.Sort((x, y) => x.Floor.CompareTo(y.Floor));

                UiDispatch(() =>
                {
                    rooms.ForEach(r => AllRooms.Add(new RoomBindableViewModel(r)));
                    CurrentlySelectedRoom = AllRooms.FirstOrDefault();
                });
            });
        }

        private void LoadFreeEquipmentTypes()
        {
            Task.Run(async () =>
            {
                var equipmentTypes = (await _equipmentTypeService.GetAll()).ToList();
                //equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                UiDispatch(() => FreeEquipmentTypes.Clear());

                equipmentTypes.ForEach(async et =>
                {
                    int amount = (await _equipmentItemService.GetAllFreeByType(et)).Count;
                    if (amount > 0)
                    {
                        UiDispatch(() => FreeEquipmentTypes.Add(new EquipmentTypeBindableViewModel(et, amount)));
                    }
                });
            });
        }

        private void LoadRoomEquipmentTypes()
        {
            Task.Run(async () =>
            {
                var equipmentTypes = (await _equipmentTypeService.GetAllByRoom(CurrentlySelectedRoom.Room)).ToList();
                equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                UiDispatch(() => EquipmentTypesInRoom.Clear());

                equipmentTypes.ForEach(async et =>
                {
                    int amount = (await _equipmentItemService.GetAllByTypeInRoom(et, CurrentlySelectedRoom.Room)).Count;
                    UiDispatch(() => EquipmentTypesInRoom.Add(new EquipmentTypeBindableViewModel(et, amount)));
                });
            });
        }

        private void ExecuteAddEquipmentItemToRoom()
        {
            UiDispatch(() =>
            {
                var typeToRemove = RemovedEquipmentTypes.FirstOrDefault(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name);
                if (typeToRemove != null)
                    RemovedEquipmentTypes.Remove(typeToRemove);

                AddedEquipmentTypes.Add(CurrentlySelectedFreeEquipmentType.EquipmentType);

                FreeEquipmentTypes.First(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name).Amount--;

                // The type is not present in the room
                if (EquipmentTypesInRoom.FirstOrDefault(et => et.Name == CurrentlySelectedFreeEquipmentType.EquipmentType.Name) == null)
                {
                    EquipmentTypesInRoom.Add(new EquipmentTypeBindableViewModel(CurrentlySelectedFreeEquipmentType.EquipmentType, 1));
                }
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
            });
        }

        private void ExecuteRemoveEquipmentItemFromRoom()
        {
            UiDispatch(() =>
            {
                var typeToRemove = AddedEquipmentTypes.FirstOrDefault(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name);
                if (typeToRemove != null)
                    AddedEquipmentTypes.Remove(typeToRemove);

                RemovedEquipmentTypes.Add(CurrentlySelectedEquipmentTypeInRoom.EquipmentType);

                EquipmentTypesInRoom.First(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name).Amount--;

                // The type is not present in the room
                if (FreeEquipmentTypes.FirstOrDefault(et => et.Name == CurrentlySelectedEquipmentTypeInRoom.EquipmentType.Name) == null)
                {
                    FreeEquipmentTypes.Add(new EquipmentTypeBindableViewModel(CurrentlySelectedEquipmentTypeInRoom.EquipmentType, 1));
                }
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
            });
        }

        private void ExecuteLoadRoomsAvailableToJoinTo()
        {
            Task.Run(async () =>
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
                
                UiDispatch(() =>
                {
                    RoomsAvailableToJoinTo.Clear();
                    freeRooms.ForEach(r => RoomsAvailableToJoinTo.Add(new RoomBindableViewModel(r)));
                });
            });
        }

        private void ExecuteScheduleRenovation()
        {
            Task.Run(async () =>
            {
                RoomAlreadyInUse = false;
                InvalidTimeFrame = false;

                var renovationStartPeriod = RenovationStartDate + RenovationStartTime?.TimeOfDay;
                var renovationEndPeriod = RenovationEndDate + RenovationEndTime?.TimeOfDay;

                if (renovationStartPeriod == null || renovationEndPeriod == null) return;

                if (renovationEndPeriod <= renovationStartPeriod)
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

                var countByRemovedEquipmentType = RemovedEquipmentTypes
                    .GroupBy(x => x)
                    .Select(g => new { Value = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                var removedItems = new List<EquipmentItem>();

                countByRemovedEquipmentType.ForEach(async pair =>
                {
                    removedItems.AddRange((await _equipmentItemService.GetAllByTypeInRoom(pair.Value, CurrentlySelectedRoom.Room)).Take(pair.Count).ToList());
                });


                if (OtherRenovations)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value);
                }

                if (NewRoomType == null)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value, addedItems, removedItems);
                }
                else
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, NewRoomType.Value, renovationStartPeriod.Value, renovationEndPeriod.Value, addedItems, removedItems);
                }

                // Create a new renovation which splits the selected room in two
                if (SplittingRoom)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value, SplittingRoom);
                }
                // Create a new renovation which extends the selected room with another room
                else if (RoomToJoinTo != null)
                {
                    await _renovationService.Create(CurrentlySelectedRoom.Room, RoomToJoinTo.Room, renovationStartPeriod.Value, renovationEndPeriod.Value);
                }

                Calendar.AddEvent((await _calendarEntryService.GetAllByRoomAndTimeFrame(CurrentlySelectedRoom.Room, renovationStartPeriod.Value, renovationEndPeriod.Value)).FirstOrDefault());
                RenovationStartDate = RenovationEndDate = RenovationStartTime = RenovationEndTime = null;
                NewRoomType = null;
            });

            Task.Run(async () =>
            {
                var equipmentTypes = (await _equipmentTypeService.GetAllByRoom(CurrentlySelectedRoom.Room)).ToList();

                equipmentTypes.ForEach(async et =>
                {
                    int amount = (await _equipmentItemService.GetAllByTypeInRoom(et, CurrentlySelectedRoom.Room)).Count;
                });
            });
        }
    }
}