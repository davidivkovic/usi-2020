using System;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace HospitalCalendar.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;
        private User _currentUser;

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


        public MainViewModel()
        {
            CurrentUser = null;
            CurrentViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();
            MessengerInstance.Register<UserLoginSuccess>(this, ExecuteLogin);
            Logout = new RelayCommand(ExecuteLogout);

            /*
            var paletteHelper = new PaletteHelper();
            //Retrieve the app's existing theme
            ITheme theme = paletteHelper.GetTheme();

            //Change the base theme to Dark
            theme.SetBaseTheme(Theme.Dark);
            //or theme.SetBaseTheme(Theme.Light);

            //Change the app's current theme
            paletteHelper.SetTheme(theme);
            */
        }

        private void ExecuteLogout()
        {
            CurrentUser = null;
            CurrentViewModel = SimpleIoc.Default.GetInstance<LoginViewModel>();
        }

        private void ExecuteLogin(UserLoginSuccess obj)
        {
            CurrentUser = obj.User;

            if (obj.User is Administrator administrator)
            {
                CurrentViewModel = SimpleIoc.Default.GetInstance<AdministratorViewModel>();
                MessengerInstance.Send(new CurrentUser(administrator));
            }
        }
    }
}
