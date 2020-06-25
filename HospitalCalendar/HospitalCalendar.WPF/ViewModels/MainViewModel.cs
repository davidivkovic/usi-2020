using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.DoctorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using HospitalCalendar.WPF.ViewModels.ManagerMenu;
using HospitalCalendar.WPF.ViewModels.SecretaryMenu;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public User CurrentUser { get; set; }
        public bool DarkModeIsEnabled { get; set; }
        public ICommand Logout { get; set; }
        public ICommand ToggleDarkMode { get; }
        public ViewModelLocator ViewModelLocator { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }
        public AdministratorViewModel AdministratorViewModel { get; set; }
        public ManagerMenuViewModel ManagerMenuViewModel { get; set; }
        public DoctorMenuViewModel DoctorMenuViewModel { get; set; }
        public SecretaryMenuViewModel SecretaryMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }

        public MainViewModel(ISynchronizationService synchronizationService ,LoginViewModel loginViewModel)
        {
            synchronizationService.Synchronize();
            ViewModelLocator = new ViewModelLocator();
            Logout = new RelayCommand(ExecuteLogout);
            ToggleDarkMode = new RelayCommand(ExecuteToggleDarkMode);

            CurrentUser = null;
            DarkModeIsEnabled = false;

            LoginViewModel = loginViewModel;
            CurrentViewModel = LoginViewModel;

            MessengerInstance.Register<UserLoginSuccess>(this, ExecuteLogin);
        }

        private void ExecuteLogout()
        {
            CurrentViewModel.Cleanup();
            if (DarkModeIsEnabled)
            {
                DarkModeIsEnabled = false;
                ExecuteToggleDarkMode();
            }

            CurrentUser = null;
            CurrentViewModel = LoginViewModel;
        }

        private void ExecuteToggleDarkMode()
        {
            var paletteHelper = new PaletteHelper();
            var theme = paletteHelper.GetTheme();
            theme.SetBaseTheme(DarkModeIsEnabled ? Theme.Dark : Theme.Light);
            paletteHelper.SetTheme(theme);

            MessengerInstance.Send(new DarkModeToggled());
        }

        private void ExecuteLogin(UserLoginSuccess message)
        {
            CurrentUser = message.User;

            switch (message.User)
            {
                case Administrator administrator:
                    CurrentViewModel = ViewModelLocator.AdministratorViewModel;
                    MessengerInstance.Send(new CurrentUser(administrator));
                    break;
                case Manager manager:
                    CurrentViewModel = ViewModelLocator.ManagerMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(manager));
                    break;
                case Doctor doctor:
                    CurrentViewModel = ViewModelLocator.DoctorMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(doctor));
                    break;
                case Secretary secretary:
                    CurrentViewModel = ViewModelLocator.SecretaryMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(secretary));
                    break;
                case Patient patient:
                    CurrentViewModel = ViewModelLocator.PatientMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(patient));
                    break;
            }
        }
    }
}