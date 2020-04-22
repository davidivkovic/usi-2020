using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.EntityFramework.Exceptions.HospitalCalendar.Domain.Exceptions;
using HospitalCalendar.EntityFramework.Services;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserRegisterViewModel : ViewModelBase
    {
        private string _firstName;
        private string _lastName;
        private string _username;
        private string _password;
        private string _confirmPassword;
        private bool _validationError;
        private bool _usernameAlreadyExists;
        private bool _nonMatchingPasswords;
        private User _selectedRole;
        private readonly IUserService _userService;

        public List<User> Roles { get; set; }
        public ICommand RegisterUser { get; private set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName == value) return;
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName == value) return;
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username == value) return;
                _username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password == value) return;
                _password = value;
                RaisePropertyChanged(nameof(Password));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword == value) return;
                _confirmPassword = value;
                RaisePropertyChanged(nameof(ConfirmPassword));
            }
        }

        public User SelectedRole
        {
            get => _selectedRole;
            set
            {
                if (_selectedRole == value) return;
                _selectedRole = value;
                RaisePropertyChanged(nameof(SelectedRole));
            }
        }

        public bool ValidationError
        {
            get => _validationError;
            set
            {
                if (_validationError == value) return;
                _validationError = value;
                RaisePropertyChanged(nameof(ValidationError));
            }
        }

        public bool UsernameAlreadyExists
        {
            get => _usernameAlreadyExists;
            set
            {
                if (_usernameAlreadyExists == value) return;
                _usernameAlreadyExists = value;
                RaisePropertyChanged(nameof(UsernameAlreadyExists));
            }
        }

        public bool NonMatchingPasswords
        {
            get => _nonMatchingPasswords;
            set
            {
                if (_nonMatchingPasswords == value) return;
                _nonMatchingPasswords = value;
                RaisePropertyChanged(nameof(NonMatchingPasswords));
            }
        }

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
            string[] inputs = {FirstName, LastName, Username, Password, ConfirmPassword};
            var userToRegister = SelectedRole;

            Task.Run(() =>
            {
                ValidationError = false;
                UsernameAlreadyExists = false;
                NonMatchingPasswords = false;

                try
                {
                    if (inputs.Any(string.IsNullOrWhiteSpace))
                    {
                        throw new ArgumentNullException();
                    }

                    if (Password != ConfirmPassword)
                    {
                        throw new NonMatchingPasswordException(Password);
                    }

                    var genericRegisterMethod = _userService.GetType().GetMethod("Register");
                    var userType = userToRegister?.GetType();
                    var typedRegisterMethod = genericRegisterMethod?.MakeGenericMethod(userType);

                    var registeredUser = (typedRegisterMethod?.Invoke(_userService, new object[] {FirstName, LastName, Username, Password}) as Task<User>)?.GetAwaiter().GetResult();

                    FirstName = LastName = Username = Password = ConfirmPassword = "";
                    SelectedRole = null;

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