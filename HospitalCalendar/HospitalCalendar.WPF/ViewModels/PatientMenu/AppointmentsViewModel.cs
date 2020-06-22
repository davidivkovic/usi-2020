
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Enums;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.PatientMenu
{
    public class AppointmentsViewModel : ViewModelBase
    {
        private readonly ICalendarEntryService _calendarEntryService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;
        public Patient Patient { get; set; }
        public Calendar Calendar { get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }
        public ICommand RequestAppointment { get; set; }
        public DateTime? LatestAppointmentDate { get; set; }
        public DateTime? AppointmentStartTime { get; set; }
        public DateTime? AppointmentEndTime { get; set; }
        public List<Doctor> AllDoctors { get; set; }
        public Doctor SelectedDoctor { get; set; }
        public IEnumerable<AppointmentPriority> Priorities { get; set; }
        public AppointmentPriority SelectedPriority { get; set; }
        public bool CanScheduleAppointment { get; set; }
        public CalendarEntry CurrentlySelectedCalendarEntry { get; set; }

        public AppointmentsViewModel(ICalendarEntryService calendarEntryService, IAppointmentService appointmentService, IDoctorService doctorService)
        {
            _calendarEntryService = calendarEntryService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;

            Priorities = Enum.GetValues(typeof(AppointmentPriority)).Cast<AppointmentPriority>();

            MessengerInstance.Register<CalendarEventUnselected>(this, HandleCalendarEventUnselected);
            MessengerInstance.Register<CalendarEventSelected>(this, HandleCalendarEventSelected);

            Calendar = new Calendar(DateTime.Now);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);
            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            RequestAppointment = new RelayCommand(ExecuteRequestAppointment);
        }

        private async void ExecuteRequestAppointment()
        {
            if (AppointmentStartTime == null || AppointmentEndTime == null || LatestAppointmentDate == null) return;
            var success = await _appointmentService.CreateOnPatientRequest(AppointmentStartTime.Value.TimeOfDay, AppointmentEndTime.Value.TimeOfDay,
                LatestAppointmentDate.Value, Patient, SelectedDoctor, SelectedPriority);
            await CleanUp();
        }

        public async void Initialize()
        {
            await LoadDoctors();
            await CleanUp();
        }

        private async Task CleanUp()
        {
            LatestAppointmentDate = AppointmentStartTime = AppointmentEndTime = null;
            SelectedDoctor = null;
            CanScheduleAppointment = (await _calendarEntryService.GetAllByPatient(Patient)).Count(appointment => appointment.StartDateTime >= DateTime.Now) <= 2;
            await LoadCurrentCalendarWeekForPatient();
        }

        public async Task LoadDoctors()
        {
            AllDoctors = (await _doctorService.GetAll())
                .Where(doctor => !doctor.Specializations
                    .All(specialization => specialization.SingleSpecialization.ToString()
                        .Contains("Surgery"))).ToList();
        }
        private async Task LoadCurrentCalendarWeekForPatient()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByPatientAndTimeFrame(Patient, weekStart, weekStart + TimeSpan.FromDays(7))).Cast<CalendarEntry>().ToList();
            Calendar.LoadCurrentWeek(entries);
        }

        private async void ExecuteLoadPreviousCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByPatientAndTimeFrame(Patient, weekStart - TimeSpan.FromDays(8), weekStart + TimeSpan.FromDays(1))).Cast<CalendarEntry>().ToList();
            Calendar.LoadPreviousWeek(entries);
        }

        private async void ExecuteLoadNextCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByPatientAndTimeFrame(Patient, weekStart + TimeSpan.FromDays(6), weekStart + TimeSpan.FromDays(15))).Cast<CalendarEntry>().ToList();
            Calendar.LoadNextWeek(entries);
        }
        private void HandleCalendarEventSelected(CalendarEventSelected message)
        {
            CurrentlySelectedCalendarEntry = message.CalendarEntry;
        }
        private void HandleCalendarEventUnselected(CalendarEventUnselected message)
        {
            if (CurrentlySelectedCalendarEntry?.ID == message.CalendarEntry.ID)
                CurrentlySelectedCalendarEntry = null;
        }
    }
}
