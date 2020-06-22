using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.PatientMenu
{
    public class PatientMenuViewModel : ViewModelBase
    {
        public ICommand ShowAppointmentsMenu { get; set; }
        public ICommand ShowAnamnesisMenu { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }
        public AppointmentsViewModel AppointmentsViewModel { get; set; }
        public AnamnesisViewModel AnamnesisViewModel { get; set; }

        public PatientMenuViewModel(AppointmentsViewModel appointmentsViewModel, AnamnesisViewModel anamnesisViewModel)
        {
            MessengerInstance.Register<CurrentUser>(this, message =>
            {
                anamnesisViewModel.Patient = message.User as Patient;
                appointmentsViewModel.Patient = message.User as Patient;
                AnamnesisViewModel = anamnesisViewModel;
                AppointmentsViewModel = appointmentsViewModel;
                ExecuteShowAnamnesisMenu();
            });

            ShowAnamnesisMenu = new RelayCommand(ExecuteShowAnamnesisMenu);
            ShowAppointmentsMenu = new RelayCommand(ExecuteShowAppointmentsMenu);
        }

        private void ExecuteShowAppointmentsMenu()
        {
            if (CurrentViewModel == AppointmentsViewModel) return;
            AppointmentsViewModel.Initialize();
            CurrentViewModel = AppointmentsViewModel;
        }

        private void ExecuteShowAnamnesisMenu()
        {
            if (CurrentViewModel == AnamnesisViewModel) return;
            AnamnesisViewModel.Initialize();
            CurrentViewModel = AnamnesisViewModel;
        }
    }
}
