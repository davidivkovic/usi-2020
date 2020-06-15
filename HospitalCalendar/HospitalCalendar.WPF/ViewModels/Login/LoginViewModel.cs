using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Exceptions;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework.Exceptions.HospitalCalendar.Domain.Exceptions;
using HospitalCalendar.WPF.Messages;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospitalCalendar.WPF.ViewModels.Login
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        public ICommand Login { get; }
        public string Username { get; set; }
        public bool InvalidCredentials { get; set; }
        public bool IsBusy { get; set; }

        public LoginViewModel(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            Login = new RelayCommand<PasswordBox>(passwordBox => ExecuteLogin(passwordBox.Password));
        }

        private async void ExecuteLogin(string password)
        {
            IsBusy = true;
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
            finally
            {
                IsBusy = false;
            }
        }
    }
}