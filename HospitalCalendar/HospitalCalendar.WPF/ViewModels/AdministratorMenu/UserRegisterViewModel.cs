using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserRegisterViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        public ICommand RegisterUser { get; }
        public List<Type> Roles { get; set; }
        public Type UserToRegister { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ValidationError { get; set; }
        public bool UsernameAlreadyExists { get; set; }
        public bool NonMatchingPasswords { get; set; }

        public UserRegisterViewModel(IUserService userService)
        {
            _userService = userService;

            RegisterUser = new RelayCommand(ExecuteRegistration);

            Roles = new List<Type>
            {
                typeof(Doctor),
                typeof(Secretary),
                typeof(Doctor)
            };
        }

        private async void ExecuteRegistration()
        {
            var inputFields = new List<string> { FirstName, LastName, Username, Password, ConfirmPassword };

            ValidationError = false;
            UsernameAlreadyExists = false;
            NonMatchingPasswords = false;

            try
            {
                if (inputFields.Any(string.IsNullOrWhiteSpace) || UserToRegister == null)
                {
                    throw new ArgumentNullException();
                }

                if (Password != ConfirmPassword)
                {
                    throw new NonMatchingPasswordException(Password);
                }

                var registeredUser = await _userService.Register(UserToRegister, FirstName, LastName, Username, Password);
                MessengerInstance.Send(new UserRegisterSuccess(registeredUser));

                Clear();
            }
            catch (ArgumentNullException)
            {
                ValidationError = true;
            }
            catch (NonMatchingPasswordException)
            {
                NonMatchingPasswords = true;
            }
            catch (UsernameAlreadyExistsException)
            {
                UsernameAlreadyExists = true;
            }
        }
        private void Clear()
        {
            FirstName = LastName = Username = Password = ConfirmPassword = string.Empty;
            UserToRegister = null;
        }
    }
}