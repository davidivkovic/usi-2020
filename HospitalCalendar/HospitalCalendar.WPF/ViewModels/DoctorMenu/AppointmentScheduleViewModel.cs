using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class AppointmentScheduleViewModel : ViewModelBase
    {
        private readonly ICalendarEntryService _calendarEntryService;
        private readonly IPatientService _patientService;
        private readonly IAnamnesisService _anamnesisService;

        public ICommand NextWeek { get; set; }
        public ICommand PreviousWeek { get; set; }
        public ICommand SaveEntry { get; set; }
        public Calendar Calendar { get; set; }
        public Doctor Doctor { get; set; }
        public Patient CurrentlySelectedPatient { get; set; }
        public ObservableCollection<Entry> Anamnesis { get; set; }
        public Entry CurrentlySelectedEntry { get; set; }
        public CalendarEntry CurrentlySelectedCalendarEntry { get; set; }
        public Specialization AppointmentSpecialization { get; set; }
        public bool AnamnesisIsEditable { get; set; }
        public string EntryText { get; set; }

        public AppointmentScheduleViewModel(ICalendarEntryService calendarEntryService, IPatientService patientService, IAnamnesisService anamnesisService)
        {
            _calendarEntryService = calendarEntryService;
            _patientService = patientService;
            _anamnesisService = anamnesisService;

            MessengerInstance.Register<CalendarEventUnselected>(this, HandleCalendarEventUnselected);
            MessengerInstance.Register<CalendarEventSelected>(this, HandleCalendarEventSelected);

            Calendar = new Calendar(DateTime.Today);
            Anamnesis = new ObservableCollection<Entry>();
            NextWeek = new RelayCommand(ExecuteLoadNextCalendarWeek);
            PreviousWeek = new RelayCommand(ExecuteLoadPreviousCalendarWeek);
            SaveEntry = new RelayCommand(ExecuteSaveEntry);
        }

        private async void ExecuteSaveEntry()
        {
            var appointment = (Appointment) CurrentlySelectedCalendarEntry;
            appointment.Type = new Specialization
            {
                IsActive = true,
                SingleSpecialization = AppointmentSpecialization.SingleSpecialization
            };
            var createdEntry = await _anamnesisService.AddEntry(appointment, EntryText, DateTime.Now);
            EntryText = string.Empty;
            Anamnesis.Add(createdEntry);
        }

        public void Initialize()
        {
            CurrentlySelectedCalendarEntry = null;
            AnamnesisIsEditable = false;
            AppointmentSpecialization = Doctor.Specializations.FirstOrDefault();
            LoadCurrentCalendarWeekForDoctor();
        }

        private async void OnCurrentlySelectedCalendarEntryChanged()
        {
            if (CurrentlySelectedCalendarEntry == null) return;

            if (CurrentlySelectedCalendarEntry.Status == AppointmentStatus.InProgress)
            {
                AnamnesisIsEditable = true;
                var selectedPatient = ((Appointment)CurrentlySelectedCalendarEntry).Patient;
                if (selectedPatient == null) return;
                CurrentlySelectedPatient = selectedPatient;
                var entries = await _anamnesisService.GetAllByPatient(selectedPatient);
                Anamnesis.Clear();
                entries.ToList().ForEach(Anamnesis.Add);
            }
            else
            {
                AnamnesisIsEditable = false;
            }
        }

        private void HandleCalendarEventSelected(CalendarEventSelected message)
        {
            CurrentlySelectedCalendarEntry = message.CalendarEntry;
        }

        private void HandleCalendarEventUnselected(CalendarEventUnselected message)
        {
            if (CurrentlySelectedCalendarEntry?.ID == message.CalendarEntry.ID && !AnamnesisIsEditable)
                CurrentlySelectedCalendarEntry = null;
        }

        private async void LoadCurrentCalendarWeekForDoctor()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = await _calendarEntryService
                .GetAllByDoctorAndTimeFrame(Doctor, weekStart, weekStart + TimeSpan.FromDays(7));
            Calendar.LoadCurrentWeek(entries.Cast<CalendarEntry>().ToList());
        }

        private async void ExecuteLoadPreviousCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByDoctorAndTimeFrame(Doctor, weekStart - TimeSpan.FromDays(8), weekStart + TimeSpan.FromDays(1)))
                .Cast<CalendarEntry>().ToList();
            Calendar.LoadPreviousWeek(entries);
        }

        private async void ExecuteLoadNextCalendarWeek()
        {
            var weekStart = Calendar.CurrentWeek.WeekStartDateTime;
            var entries = (await _calendarEntryService
                .GetAllByDoctorAndTimeFrame(Doctor, weekStart + TimeSpan.FromDays(6), weekStart + TimeSpan.FromDays(15)))
                .Cast<CalendarEntry>().ToList();
            Calendar.LoadNextWeek(entries);
        }
    }
}