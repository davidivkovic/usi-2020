using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.DoctorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using HospitalCalendar.WPF.ViewModels.ManagerMenu;
using HospitalCalendar.WPF.ViewModels.SecretaryMenu;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.DependencyInjection;
using PropertyChanged;

namespace HospitalCalendar.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public User CurrentUser { get; set; }
        public bool DarkModeIsEnabled { get; set; }

        public ICommand Logout { get; set; }
        public ICommand ToggleDarkMode { get; }

        public ViewModelBase CurrentViewModel { get; set; }
        public AdministratorViewModel AdministratorViewModel { get; set; }
        public ManagerMenuViewModel ManagerMenuViewModel { get; set; }
        public DoctorMenuViewModel DoctorMenuViewModel { get; set; }
        public SecretaryMenuViewModel SecretaryMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }

        public MainViewModel(LoginViewModel loginViewModel)
        {
            DarkModeIsEnabled = false;
            ToggleDarkMode = new RelayCommand(ExecuteToggleDarkMode);

            LoginViewModel = loginViewModel;

            CurrentUser = null;
            CurrentViewModel = LoginViewModel;
            MessengerInstance.Register<UserLoginSuccess>(this, ExecuteLogin);
            Logout = new RelayCommand(ExecuteLogout);
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
                    CurrentViewModel = new ViewModelLocator().AdministratorViewModel;
                    MessengerInstance.Send(new CurrentUser(administrator));
                    break;
                case Manager manager:
                    CurrentViewModel = new ViewModelLocator().ManagerMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(manager));
                    break;
                case Doctor doctor:
                    CurrentViewModel = new ViewModelLocator().DoctorMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(doctor));
                    break;
                case Secretary secretary:
                    CurrentViewModel = new ViewModelLocator().SecretaryMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(secretary));
                    break;
            }
        }
    }
}