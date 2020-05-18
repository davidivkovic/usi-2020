using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using HospitalCalendar.WPF.ViewModels.ManagerMenu;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
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
        public LoginViewModel LoginViewModel { get; set; }

        public MainViewModel(AdministratorViewModel administratorViewModel, ManagerMenuViewModel managerMenuViewModel, LoginViewModel loginViewModel)
        {
            DarkModeIsEnabled = false;

            AdministratorViewModel = administratorViewModel;
            ManagerMenuViewModel = managerMenuViewModel;
            LoginViewModel = loginViewModel;

            ToggleDarkMode = new RelayCommand(ExecuteToggleDarkMode);

            CurrentUser = null;
            CurrentViewModel = LoginViewModel;
            MessengerInstance.Register<UserLoginSuccess>(this, ExecuteLogin);
            Logout = new RelayCommand(ExecuteLogout);
        }

        private void ExecuteLogout()
        {
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
            //Retrieve the app's existing theme
            ITheme theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(DarkModeIsEnabled ? Theme.Dark : Theme.Light);

            //Change the app's current theme
            paletteHelper.SetTheme(theme);

            MessengerInstance.Send(new DarkModeToggled());
        }

        private void ExecuteLogin( UserLoginSuccess message)
        {
            CurrentUser = message.User;

            switch (message.User)
            {
                case Administrator administrator:
                    CurrentViewModel = AdministratorViewModel;
                    MessengerInstance.Send(new CurrentUser(administrator));
                    break;
                case Manager manager:
                    CurrentViewModel = ManagerMenuViewModel;
                    MessengerInstance.Send(new CurrentUser(manager));
                    break;
            }
        }
    }
}
