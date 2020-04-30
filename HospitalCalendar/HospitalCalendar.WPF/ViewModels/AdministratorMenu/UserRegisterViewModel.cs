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

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserRegisterViewModel : ViewModelBase
    {
        private User _userToRegister;
        private string _password;
        private string _confirmPassword;
        private bool _validationError;
        private bool _usernameAlreadyExists;
        private bool _nonMatchingPasswords;
        private readonly IUserService _userService;

        public List<User> Roles { get; set; }

        public ICommand RegisterUser { get; private set; }

        public User UserToRegister
        {
            get => _userToRegister;
            set
            {
                _userToRegister = value;
                RaisePropertyChanged(nameof(UserToRegister));
                RaisePropertyChanged(nameof(FirstName));
                RaisePropertyChanged(nameof(LastName));
                RaisePropertyChanged(nameof(Username));
            }
        }

        public string FirstName
        {
            get => _userToRegister?.FirstName;
            set
            {
                if (_userToRegister.FirstName == value) return;
                _userToRegister.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _userToRegister?.LastName;
            set
            {
                if (_userToRegister.LastName == value) return;
                _userToRegister.LastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string Username
        {
            get => _userToRegister?.Username;
            set
            {
                if (_userToRegister.Username == value) return;
                _userToRegister.Username = value;
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
            string[] inputFields = {FirstName, LastName, Username, Password, ConfirmPassword};

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