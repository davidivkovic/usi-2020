using System.Windows;
using System.Windows.Controls;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.Views.DoctorMenu
{
    public partial class AppointmentCreate : UserControl
    {
        public AppointmentCreate()
        {
            InitializeComponent();
            AvailableSpecialists.IsEnabled = false;
            ScheduleAppointmentAtSpecialist.Checked += (o, e) =>
            {
                AvailableSpecialists.IsEnabled = true;
                ScheduleButton.IsEnabled = false;
            };

            RoomList.ComboBoxSelectionChanged += (o, e) =>
            {
                UrgentCheckBox.IsChecked = false;
                UrgentCheckBox.Visibility = ((Room)RoomList.SelectedItem).Type == RoomType.Surgery ? Visibility.Visible : Visibility.Hidden;
            };

             ScheduleAppointmentAtSpecialist.Unchecked += (o, e) =>
            {
                AvailableSpecialists.IsEnabled = false;
                ScheduleButton.IsEnabled = true;
            };

            AppointmentStartDate.IsEnabled = AppointmentEndDate.IsEnabled = AppointmentStartTime.IsEnabled = AppointmentEndTime.IsEnabled = false;

            AppointmentStartDate.SelectedDateChanged += (o, e) => ResetSpecialists();
            AppointmentEndDate.SelectedDateChanged += (o, e) => ResetSpecialists();
            AppointmentStartTime.SelectedTimeChanged += (o, e) => ResetSpecialists();
            AppointmentEndTime.SelectedTimeChanged += (o, e) => ResetSpecialists();

            PatientsList.SelectionChanged += (o, e) =>
            {
                AppointmentStartDate.IsEnabled = AppointmentEndDate.IsEnabled = AppointmentStartTime.IsEnabled = AppointmentEndTime.IsEnabled = true;
            };

            AvailableSpecialists.SelectionChanged += (o, e) =>
            {
                ScheduleButton.IsEnabled = true;
            };
        }
        private void ResetSpecialists()
        {
            AvailableSpecialists.SelectedItem = null;
            ScheduleAppointmentAtSpecialist.IsChecked = false;
        }
    }
}