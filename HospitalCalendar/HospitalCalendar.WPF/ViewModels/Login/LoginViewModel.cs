
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
using HospitalCalendar.WPF.Messages;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace HospitalCalendar.WPF.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        public ICommand Login { get; }
        public string Username { get; set; }
        public bool InvalidCredentials { get; set; }

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            Login = new RelayCommand<PasswordBox>(async(passwordBox) => await ExecuteLogin(passwordBox.Password));
        }

        private async Task ExecuteLogin(string password)
        {
            InvalidCredentials = false;
            try
            {
                var user = await _authenticationService.Login(Username, password);
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
        }
    }
}
