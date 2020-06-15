using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using MaterialDesignThemes.Wpf;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class AppointmentCreateViewModel : ViewModelBase
    {
        private readonly IPatientService _patientService;
        private readonly IRoomService _roomService;
        private readonly ICalendarEntryService _calendarEntryService;
        private readonly IDoctorService _doctorService;

        public Doctor Doctor { get; set; }

        [AlsoNotifyFor(nameof(FilteredPatients))]
        public string FilterText { get; set; }

        [AlsoNotifyFor(nameof(FilteredPatients))]
        public ObservableCollection<Patient> AllPatients { get; set; }
        public Patient SelectedPatient { get; set; }
        public ObservableCollection<Room> AvailableRooms { get; set; }
        public Room SelectedRoom { get; set; }
        public List<Type> AppointmentTypes { get; set; }
        public RoomType SelectedAppointmentType { get; set; }
        public DateTime? AppointmentStartDate { get; set; }
        public DateTime? AppointmentEndDate { get; set; }
        public DateTime? AppointmentStartTime { get; set; }
        public DateTime? AppointmentEndTime { get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }
        public ICommand ScheduleAppointment { get; set; }
        public Calendar Calendar { get; set; }
        public ObservableCollection<Doctor> AvailableSpecialists { get; set; }
        public Doctor SelectedSpecialist { get; set; }
        public bool ScheduleAppointmentAtSpecialist { get; set; }
        public bool RoomAlreadyInUse { get; set; }
        public bool InvalidTimeFrame { get; set; }
        public bool IsUrgent { get; set; }
        public SnackbarMessageQueue MaterialDesignMessageQueue { get; set; }


        public ObservableCollection<Patient> FilteredPatients
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return AllPatients;

                var filterResults = AllPatients
                    .Where(p => FilterText.Split().All(p.InsuranceNumber.Contains) ||
                                FilterText.ToLower().Split().Any(p.FirstName.ToLower().Contains) ||
                                FilterText.ToLower().Split().Any(p.LastName.ToLower().Contains));

                return new ObservableCollection<Patient>(filterResults);
            }
        }

        // Used by Fody Weaver
        private void OnScheduleAppointmentAtSpecialistChanged()
        {
            var appointmentStart = AppointmentStartDate + AppointmentStartTime?.TimeOfDay;
            var appointmentEnd = AppointmentEndDate + AppointmentEndTime?.TimeOfDay;

            if (appointmentEnd == null || appointmentStart == null)
                return;
            LoadAvailableSpecialists(appointmentStart.Value, appointmentEnd.Value);
        }

        // Used by Fody Weaver
        private async void OnSelectedRoomChanged()
        {
            if (SelectedRoom == null) return;
            SelectedAppointmentType = SelectedRoom.Type;
            await LoadCurrentCalendarWeekForRoom();
            ScheduleAppointmentAtSpecialist = false;
        }

        public AppointmentCreateViewModel(IPatientService patientService, IRoomService roomService, ICalendarEntryService calendarEntryService, IDoctorService doctorService)
        {
            _patientService = patientService;
            _roomService = roomService;
            _calendarEntryService = calendarEntryService;
            _doctorService = doctorService;

            AvailableRooms = new ObservableCollection<Room>();
            AllPatients = new ObservableCollection<Patient>();
            AvailableSpecialists = new ObservableCollection<Doctor>();

            Calendar = new Calendar(DateTime.Today);
            ScheduleAppointment = new RelayCommand(ExecuteScheduleAppointment);
            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);

            MaterialDesignMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(4))
            {
                IgnoreDuplicate = true
            };
        }
        public async void Initialize()
        {
            await LoadPatients();
            await LoadRooms();
            await LoadCurrentCalendarWeekForRoom();
            Doctor = await _doctorService.Get(Doctor.ID);
            SelectedAppointmentType = AvailableRooms.First().Type;
            Calendar.SetWorkingHours(Doctor.WorkingHoursStart, Doctor.WorkingHoursEnd);
        }

        private void Clean()
        {
            FilterText = string.Empty;
            AppointmentStartDate = AppointmentEndDate = AppointmentStartTime = AppointmentEndTime = null;
            SelectedPatient = null;
        }
        public async void LoadAvailableSpecialists(DateTime appointmentStart, DateTime appointmentEnd)
        {
            if (SelectedRoom == null) return;
            List<Doctor> availableSpecialists;

            if (SelectedRoom.Type == RoomType.Surgery)
            {
                availableSpecialists = (await _doctorService.GetAllFree(appointmentStart, appointmentEnd))
                    .Where(doctor => doctor.Specializations
                        .Any(specialization => specialization.SingleSpecialization.ToString()
                            .Contains("Surgery")))
                    .ToList();
            }
            else
            {
                availableSpecialists = (await _doctorService.GetAllFree(appointmentStart, appointmentEnd))
                    .Where(doctor => !doctor.Specializations
                        .Any(specialization => specialization.SingleSpecialization.ToString()
                            .Contains("Surgery")))
                    .ToList();
            }

            AvailableSpecialists.Clear();
            availableSpecialists.ForEach(specialist => AvailableSpecialists.Add(specialist));
        }

        private async void ExecuteScheduleAppointment()
        {
            await Task.Run(async () =>
            {
                RoomAlreadyInUse = InvalidTimeFrame = false;
                var appointmentStart = AppointmentStartDate + AppointmentStartTime?.TimeOfDay;
                var appointmentEnd = AppointmentEndDate + AppointmentEndTime?.TimeOfDay;

                CalendarEntry created = null;

                if (appointmentEnd <= appointmentStart || appointmentEnd == null || appointmentStart == null)
                {
                    InvalidTimeFrame = true;
                }

                // Trying to save some RAM
                else if ((await _calendarEntryService.GetAllByRoomAndTimeFrame(SelectedRoom, appointmentStart.Value, appointmentEnd.Value)).ToList().Count != 0)
                {
                    //var res = await _calendarEntryService.GetAllByRoomAndTimeFrame(SelectedRoom, appointmentStart.Value, appointmentEnd.Value);
                    RoomAlreadyInUse = true;
                }

                else switch (SelectedAppointmentType)
                {
                    case RoomType.CheckUp:

                        if (ScheduleAppointmentAtSpecialist)
                        {
                            await _calendarEntryService.CreateAppointmentRequest(appointmentStart.Value, appointmentEnd.Value, SelectedPatient, Doctor, SelectedSpecialist, DateTime.Now, SelectedRoom);
                            MaterialDesignMessageQueue.Enqueue("All managers have been notified of this appointment.");
                        }
                        else
                            created = await _calendarEntryService.CreateAppointment(appointmentStart.Value, appointmentEnd.Value, Doctor, SelectedPatient, SelectedRoom);
                        Clean();
                        break;

                    case RoomType.Surgery:

                        if (ScheduleAppointmentAtSpecialist)
                        {
                            await _calendarEntryService.CreateSurgeryRequest(appointmentStart.Value, appointmentEnd.Value, SelectedPatient, Doctor, SelectedSpecialist, IsUrgent, DateTime.Now, SelectedRoom);
                            MaterialDesignMessageQueue.Enqueue("All managers have been notified of this surgery.");
                        }
                        else
                        {
                            created = await _calendarEntryService.CreateSurgery(appointmentStart.Value, appointmentEnd.Value, Doctor, SelectedPatient, SelectedRoom, IsUrgent);
                            if (IsUrgent)
                                MaterialDesignMessageQueue.Enqueue("All managers have been notified of this surgery");
                        }
                        Clean();
                        break;
                }
                if (created != null)
                {
                    Application.Current.Dispatcher.Invoke(() => Calendar.AddEvent(created));
                }
            });
        }

        private async Task LoadPatients()
        {
            var allPatients = await _patientService.GetAll();
            AllPatients = new ObservableCollection<Patient>(allPatients);
        }

        private async Task LoadRooms()
        {
            var availableRooms = (await _roomService.GetAll()).Where(r => r.Type == RoomType.CheckUp || r.Type == RoomType.Surgery).ToList();
            AvailableRooms = new ObservableCollection<Room>(availableRooms);
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
    }
}