using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using System.Windows.Input;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class DoctorMenuViewModel : ViewModelBase
    {
        public static Doctor Doctor { get; set; }
        public ICommand ShowCreateAppointmentMenu { get; set; }
        public ICommand ShowAppointmentSchedule { get; set; }
        public ICommand ShowReportMenu { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }
        public AppointmentScheduleViewModel AppointmentScheduleViewModel { get; set; }
        public AppointmentCreateViewModel AppointmentCreateViewModel { get; set; }

        public DoctorMenuViewModel(AppointmentScheduleViewModel appointmentScheduleViewModel, AppointmentCreateViewModel appointmentCreateViewModel, IDoctorService doctorService)
        {
            

            MessengerInstance.Register<CurrentUser>(this, async message =>
            {
                var foundDoctor = await doctorService.Get(message.User.ID);
                //Doctor = message.User as Doctor;
                appointmentScheduleViewModel.Doctor = foundDoctor;
                appointmentCreateViewModel.Doctor = foundDoctor;
                AppointmentScheduleViewModel = appointmentScheduleViewModel;
                AppointmentCreateViewModel = appointmentCreateViewModel;
                ExecuteShowAppointmentSchedule();
            });
            ShowAppointmentSchedule = new RelayCommand(ExecuteShowAppointmentSchedule);
            ShowCreateAppointmentMenu = new RelayCommand(ExecuteShowCreateAppointmentMenu);
        }

        private void ExecuteShowCreateAppointmentMenu()
        {
            if (CurrentViewModel == AppointmentCreateViewModel) return;
            AppointmentCreateViewModel.Initialize();
            CurrentViewModel = AppointmentCreateViewModel;
        }
        private void ExecuteShowAppointmentSchedule()
        {
            if (CurrentViewModel == AppointmentScheduleViewModel) return;
            AppointmentScheduleViewModel.Initialize();
            CurrentViewModel = AppointmentScheduleViewModel;
        }
    }
}