using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserUpdateViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _confirmPassword;
        private bool _usernameAlreadyExists;
        private bool _nonMatchingPasswords;
        private readonly IUserService _userService;

        public User UserToUpdate { get; set; }
        public ICommand UpdateUser { get; set; }

        public string FirstName
        {
            get => UserToUpdate?.FirstName;
            set
            {
                if (UserToUpdate?.FirstName == value) return;
                UserToUpdate.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => UserToUpdate?.LastName;
            set
            {
                if (UserToUpdate?.LastName == value) return;
                UserToUpdate.LastName = value;
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

        public UserUpdateViewModel(IUserService userService)
        {
            _userService = userService;
            UpdateUser = new RelayCommand(ExecuteUpdateUser);
            MessengerInstance.Register<UserUpdateRequest>(this, message =>
            {
                UserToUpdate = message.User;
                Username = UserToUpdate.Username;
                RaisePropertyChanged(nameof(FirstName));
                RaisePropertyChanged(nameof(LastName));
                RaisePropertyChanged(nameof(Username));
            });
        }

        private void ExecuteUpdateUser()
        {
            Task.Run(() =>
            {
                UsernameAlreadyExists = false;
                NonMatchingPasswords = false;
                try
                {
                    if (Password != ConfirmPassword)
                    {
                        throw new NonMatchingPasswordException(Password);
                    }

                    var updatedUser = _userService.Update(UserToUpdate, FirstName, LastName, Username, Password).GetAwaiter().GetResult();
                    MessengerInstance.Send(new UserUpdateSuccess(updatedUser));
                }
                catch (UsernameAlreadyExistsException)
                {
                    UsernameAlreadyExists = true;
                }
                catch (NonMatchingPasswordException)
                {
                    NonMatchingPasswords = true;
                }
            });
        }
    }
}
