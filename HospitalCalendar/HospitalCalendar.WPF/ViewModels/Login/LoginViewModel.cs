
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.Domain.Exceptions;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework.Exceptions.HospitalCalendar.Domain.Exceptions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace HospitalCalendar.WPF.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private string _username;
        private bool _invalidCredentials;

        public ICommand Login { get; }

        public string Username
        {
            get => _username;

            set
            {
                if (value == _username)
                    return;
                _username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }

        public bool InvalidCredentials
        {
            get => _invalidCredentials;

            set
            {
                if (value == _invalidCredentials)
                    return;
                _invalidCredentials = value;
                RaisePropertyChanged(nameof(InvalidCredentials));
            }
        }

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            Login = new RelayCommand<PasswordBox>(ExecuteLogin);
        }

        private void ExecuteLogin(PasswordBox passwordBox)
        {
            InvalidCredentials = false;

            Task.Run(() =>
            {
                try
                {
                    var user = _authenticationService.Login(Username, passwordBox.Password).GetAwaiter().GetResult();
                    MessengerInstance.Send(new UserLoginSuccess(user));
                }
                catch (InvalidUsernameException)
                {
                    InvalidCredentials = true;
                }
                catch (InvalidPasswordException)
                {
                    InvalidCredentials = true;
                }
            });

        }
    }
}
