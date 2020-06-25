using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;

namespace HospitalCalendar.WPF.ViewModels.SecretaryMenu
{
    public class SecretaryMenuViewModel : ViewModelBase
    {
        public ICommand ShowPatientRegistrationMenu { get; set; }
        public ICommand ShowNotificationsMenu { get; set; }
        public ICommand ShowScheduleMenu { get; set; }

        public static Secretary Secretary { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }
        public PatientRegisterViewModel PatientRegisterViewModel { get; set; }
        public NotificationMenuViewModel NotificationMenuViewModel { get; set; }
        public ScheduleViewModel ScheduleViewModel { get; set; }

        public SecretaryMenuViewModel(PatientRegisterViewModel patientRegisterViewModel, NotificationMenuViewModel notificationMenuViewModel, ScheduleViewModel scheduleViewModel)
        {
            PatientRegisterViewModel = patientRegisterViewModel;
            NotificationMenuViewModel = notificationMenuViewModel;
            ScheduleViewModel = scheduleViewModel;
            MessengerInstance.Register<CurrentUser>(this, message => Secretary = message.User as Secretary);
            ShowPatientRegistrationMenu = new RelayCommand(ExecuteShowPatientRegistrationMenu);
            ShowNotificationsMenu = new RelayCommand(ExecuteShowNotificationsMenu);
            ShowScheduleMenu = new RelayCommand(ExecuteShowShowScheduleMenu);
            ExecuteShowPatientRegistrationMenu();
        }

        private void ExecuteShowShowScheduleMenu()
        {
            if (CurrentViewModel == ScheduleViewModel) return;
            ScheduleViewModel.Initialize();
            CurrentViewModel = ScheduleViewModel;
        }

        private void ExecuteShowNotificationsMenu()
        {
            if (CurrentViewModel == NotificationMenuViewModel) return;
            NotificationMenuViewModel.Initialize();
            CurrentViewModel = NotificationMenuViewModel;
        }

        private void ExecuteShowPatientRegistrationMenu()
        {
            if (CurrentViewModel == PatientRegisterViewModel) return;
            PatientRegisterViewModel.Initialize();
            CurrentViewModel = PatientRegisterViewModel;
        }
    }
}
