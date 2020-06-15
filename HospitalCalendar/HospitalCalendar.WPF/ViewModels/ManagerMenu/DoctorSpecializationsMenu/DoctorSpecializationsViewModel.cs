using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.DoctorSpecializationsMenu
{
    public class DoctorSpecializationsViewModel : ViewModelBase
    {
        private readonly IDoctorService _doctorService;
        public ObservableCollection<Doctor> AllDoctors { get; set; } = new ObservableCollection<Doctor>();
        public ObservableCollection<Specializations> AllSpecializations { get; set; }
        public ObservableCollection<Specialization> DoctorsSpecializations { get; set; } = new ObservableCollection<Specialization>();
        public Doctor CurrentlySelectedDoctor { get; set; }
        public DateTime? WorkingHoursStart { get; set; }
        public DateTime? WorkingHoursEnd { get; set; }
        public Specializations CurrentlySelectedSpecialization { get; set; }
        public bool CanAddSpecialization { get; set; }
        public bool CanRemoveSpecialization { get; set; }
        public ICommand AddSpecializationToDoctor { get; set; }
        public ICommand RemoveSpecializationFromDoctor { get; set; }
        public ICommand UpdateDoctorData { get; set; }

        // These two methods are FODY WEAVER EVENT HANDLERS
        private void OnCurrentlySelectedSpecializationChanged()
        {
            if (DoctorsSpecializations.FirstOrDefault(s => s.SingleSpecialization == CurrentlySelectedSpecialization) == null)
            {
                CanAddSpecialization = true;
                CanRemoveSpecialization = false;
            }
            else
            {
                CanAddSpecialization = false;
                CanRemoveSpecialization = true;
            }
        }

        private void OnCurrentlySelectedDoctorChanged()
        {

            DoctorsSpecializations = new ObservableCollection<Specialization>();
            CurrentlySelectedDoctor?.Specializations.ToList().ForEach(s => DoctorsSpecializations.Add(s));

            OnCurrentlySelectedSpecializationChanged();
            WorkingHoursStart = DateTime.MinValue + CurrentlySelectedDoctor?.WorkingHoursStart;
            WorkingHoursEnd = DateTime.MinValue + CurrentlySelectedDoctor?.WorkingHoursEnd;
        }

        public DoctorSpecializationsViewModel(IDoctorService doctorService)
        {
            _doctorService = doctorService;
            AddSpecializationToDoctor = new RelayCommand(ExecuteAddSpecializationToDoctor);
            RemoveSpecializationFromDoctor = new RelayCommand(ExecuteRemoveSpecializationFromDoctor);
            UpdateDoctorData = new RelayCommand(ExecuteUpdateDoctorData);
            AllSpecializations = new ObservableCollection<Specializations>(Enum.GetValues(typeof(Specializations)).Cast<Specializations>().ToList().OrderBy(s => s));
        }

        public void Initialize()
        {
            CanRemoveSpecialization = false;
            CanAddSpecialization = true;
            LoadDoctors();
        }

        private async void ExecuteUpdateDoctorData()
        {
            if (WorkingHoursStart == null || WorkingHoursEnd == null) return;

            var doctorToUpdate = await _doctorService.Get(CurrentlySelectedDoctor.ID);

            doctorToUpdate.WorkingHoursStart = WorkingHoursStart.Value.TimeOfDay;
            doctorToUpdate.WorkingHoursEnd = WorkingHoursEnd.Value.TimeOfDay;

            doctorToUpdate.Specializations.Clear();

            DoctorsSpecializations.ToList().ForEach(specialization =>
            {
                doctorToUpdate.Specializations.Add(specialization);
            });

            await _doctorService.Update(doctorToUpdate, doctor => doctor.Specializations);
            LoadDoctors();
        }

        private void ExecuteRemoveSpecializationFromDoctor()
        {
            //UiDispatch(() => 
            DoctorsSpecializations.Remove(DoctorsSpecializations.First(s => s.SingleSpecialization == CurrentlySelectedSpecialization));
                //);
            OnCurrentlySelectedSpecializationChanged();
        }

        private void ExecuteAddSpecializationToDoctor()
        {
            //UiDispatch(() =>
            //{
                DoctorsSpecializations.Add(new Specialization { SingleSpecialization = CurrentlySelectedSpecialization, IsActive = true });
            //});
            OnCurrentlySelectedSpecializationChanged();
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        private async void LoadDoctors()
        {
            AllDoctors = new ObservableCollection<Doctor>();

            var allDoctors = (await _doctorService.GetAll()).ToList();
            allDoctors.ForEach(d => AllDoctors.Add(d));
        }
    }
}