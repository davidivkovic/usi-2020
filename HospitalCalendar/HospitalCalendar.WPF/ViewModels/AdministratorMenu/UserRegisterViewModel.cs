using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserRegisterViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        public List<User> Roles { get; set; }
        public ICommand RegisterUser { get; private set; }

        //[AlsoNotifyFor(nameof(FirstName), nameof(LastName), nameof(Username))]
        public User UserToRegister { get; set; }

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

            Roles = new List<User>
            {
                new Doctor(),
                new Secretary(),
                new Manager()
            };
        }

        private void ExecuteRegistration()
        {
            var inputFields = new List<string>{FirstName, LastName, Username, Password, ConfirmPassword};

            ValidationError = false;
            UsernameAlreadyExists = false;
            NonMatchingPasswords = false;

            Task.Run(() =>
            {
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
                    // TODO: This should be extracted to a separate method
                    var genericRegisterMethod = _userService.GetType().GetMethod("Register");
                    var userType = UserToRegister?.GetType();
                    var typedRegisterMethod = genericRegisterMethod?.MakeGenericMethod(userType);

                    var registeredUser = (typedRegisterMethod?.Invoke(_userService, new object[] {FirstName, LastName, Username, Password}) as Task<User>)?.GetAwaiter().GetResult();

                    FirstName = LastName = Username = Password = ConfirmPassword = "";
                    UserToRegister = null;

                    MessengerInstance.Send(new UserRegisterSuccess(registeredUser));
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
            });
        }
    }
}