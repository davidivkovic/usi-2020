using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class AppointmentCreateViewModel : ViewModelBase
    {
        private readonly IPatientService _patientService;
        private readonly IRoomService _roomService;
        public Doctor Doctor { get; set; }

        [AlsoNotifyFor(nameof(FilteredPatients))]
        public string FilterText { get; set; }

        [AlsoNotifyFor(nameof(FilteredPatients))]
        public ObservableCollection<Patient> AllPatients { get; set; } = new ObservableCollection<Patient>();
        public Patient SelectedPatient { get; set; }
        public ObservableCollection<Room> AvailableRooms { get; set; } = new ObservableCollection<Room>();
        public Room SelectedRoom { get; set; }
        public List<Appointment> AppointmentTypes { get; set; }
        public Appointment SelectedAppointmentType { get; set; }
        public DateTime? AppointmentStartDate { get; set; }
        public DateTime? AppointmentEndDate { get; set; }
        public DateTime? AppointmentStartTime{ get; set; }
        public DateTime? AppointmentEndTime { get; set; }

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

        private void OnSelectedAppointmentTypeChanged()
        {

        }

        private void OnSelectedRoomChanged()
        {

        }

        public AppointmentCreateViewModel(IPatientService patientService, IRoomService roomService)
        {
            _patientService = patientService;
            _roomService = roomService;
            MessengerInstance.Register<CurrentUser>(this, message => Doctor = message.User as Doctor);
            AppointmentTypes = new List<Appointment>
            {
                new Appointment(),
                new Surgery(),
            };
        }

        private static void UiDispatch(Action action)
        {
            Application.Current.Dispatcher.Invoke(action);
        }

        public void Initialize()
        {
            FilterText = string.Empty;
            LoadPatients();
            LoadRooms();
        }

        private void LoadPatients()
        {
            Task.Run(async () =>
            {
                var allPatients = await _patientService.GetAll();
                UiDispatch(() => AllPatients = new ObservableCollection<Patient>(allPatients));
            });
        }

        private void LoadRooms()
        {
            Task.Run(async () =>
            {
                var appointmentStartPeriod = AppointmentStartDate + AppointmentStartTime?.TimeOfDay;
                var appointmentEndPeriod = AppointmentEndDate + AppointmentEndTime?.TimeOfDay;

                if (appointmentStartPeriod == null || appointmentEndPeriod == null) return;

                var availableRooms = SelectedAppointmentType is Surgery ? 
                    (await _roomService.GetAllFree(appointmentStartPeriod.Value, appointmentEndPeriod.Value)).Where(r => r.Type == RoomType.Surgery) :
                    (await _roomService.GetAllFree(appointmentStartPeriod.Value, appointmentEndPeriod.Value)).Where(r => r.Type == RoomType.CheckUp);

                UiDispatch(() => AvailableRooms = new ObservableCollection<Room>(availableRooms));
            });
        }
    }
}