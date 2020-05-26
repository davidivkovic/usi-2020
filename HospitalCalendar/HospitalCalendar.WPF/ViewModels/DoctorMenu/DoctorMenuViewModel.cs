using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.WPF.DataTemplates.Calendar;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.DoctorSpecializationsMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class DoctorMenuViewModel : ViewModelBase
    {
        public static Doctor Doctor { get; set; }
        public ICommand ShowCreateAppointmentMenu { get; set; }
        public ICommand ShowAppointmentSchedule { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }

        public AppointmentScheduleViewModel AppointmentScheduleViewModel { get; set; }
        public AppointmentCreateViewModel AppointmentCreateViewModel { get; set; }

        public DoctorMenuViewModel(AppointmentScheduleViewModel appointmentScheduleViewModel, AppointmentCreateViewModel appointmentCreateViewModel)
        {
            AppointmentScheduleViewModel = appointmentScheduleViewModel;
            AppointmentCreateViewModel = appointmentCreateViewModel;
            ShowAppointmentSchedule = new RelayCommand(ExecuteShowAppointmentSchedule);
            ShowCreateAppointmentMenu = new RelayCommand(ExecuteShowCreateAppointmentMenu);
            ExecuteShowAppointmentSchedule();
        }

        private void ExecuteShowCreateAppointmentMenu()
        {
            if (CurrentViewModel == AppointmentCreateViewModel) return;
            AppointmentCreateViewModel.Initialize();
            CurrentViewModel = AppointmentCreateViewModel;
        }
        private void ExecuteShowAppointmentSchedule()
        {
            if(CurrentViewModel == AppointmentScheduleViewModel) return;
            AppointmentScheduleViewModel.Initialize();
            CurrentViewModel = AppointmentScheduleViewModel;
        }
    }
}
