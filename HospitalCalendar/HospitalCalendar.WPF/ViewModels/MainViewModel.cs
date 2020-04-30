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

namespace HospitalCalendar.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        private ViewModelBase _currentViewModel;
        private User _currentUser;
        private bool _darkModeIsEnabled;

        public ICommand Logout { get; set; }

        public User CurrentUser
        {
            get => _currentUser;

            set
            {
                if (_currentUser == value)
                    return;
                _currentUser = value;
                RaisePropertyChanged(nameof(CurrentUser));
            }

        }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;

            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged(nameof(CurrentViewModel));
                RaisePropertyChanged(nameof(CurrentUserFirstName));
                RaisePropertyChanged(nameof(CurrentUserLastName));
            }
        }

        public string CurrentUserFirstName
        {
            get => CurrentUser?.FirstName;

            set
            {
                if (CurrentUser?.FirstName == value)
                    return;
                CurrentUser.FirstName = value;
                RaisePropertyChanged(nameof(CurrentUserFirstName));
            }
        }

        public string CurrentUserLastName
        {
            get => CurrentUser?.LastName;

            set
            {
                if (CurrentUser?.LastName == value)
                    return;
                CurrentUser.LastName = value;
                RaisePropertyChanged(nameof(CurrentUserLastName));
            }
        }

        public bool DarkModeIsEnabled
        {
            get => _darkModeIsEnabled;

            set
            {
                if (_darkModeIsEnabled == value)
                    return;
                _darkModeIsEnabled = value;
                RaisePropertyChanged(nameof(DarkModeIsEnabled));
            }
        }

        public ICommand ToggleDarkMode { get; }
        public AdministratorViewModel AdministratorViewModel { get; set; }
        public ManagerMenuViewModel ManagerMenuViewModel { get; set; }
        public LoginViewModel LoginViewModel { get; set; }

        #endregion

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

        private void ExecuteToggleDarkMode()
        {
            var paletteHelper = new PaletteHelper();
            //Retrieve the app's existing theme
            ITheme theme = paletteHelper.GetTheme();

            theme.SetBaseTheme(DarkModeIsEnabled ? Theme.Light : Theme.Dark);
            DarkModeIsEnabled = !DarkModeIsEnabled;

            //Change the app's current theme
            paletteHelper.SetTheme(theme);
        }

        private void ExecuteLogout()
        {
            if (DarkModeIsEnabled)
            {
                ExecuteToggleDarkMode();
            }
                
            CurrentUser = null;
            CurrentViewModel = LoginViewModel;
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
