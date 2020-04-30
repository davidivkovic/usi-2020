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
    public class UserUpdateViewModel : ViewModelBase
    {
        #region Properties
        private User _userToUpdate;
        private string _password;
        private string _confirmPassword;
        private bool _usernameAlreadyExists;
        private bool _nonMatchingPasswords;
        
        private readonly IUserService _userService;

        public ICommand UpdateUser { get; set; }
        
        public User UserToUpdate
        {
            get => _userToUpdate;
            set
            {
                _userToUpdate = value;
                RaisePropertyChanged(nameof(UserToUpdate));
                RaisePropertyChanged(nameof(FirstName));
                RaisePropertyChanged(nameof(LastName));
                RaisePropertyChanged(nameof(Username));
            }
        }

        public string FirstName
        {
            get => _userToUpdate?.FirstName;
            set
            {
                if (_userToUpdate.FirstName == value) return;
                _userToUpdate.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _userToUpdate?.LastName;
            set
            {
                if (_userToUpdate.LastName == value) return;
                _userToUpdate.LastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string Username
        {
            get => _userToUpdate?.Username;
            set
            {
                if (_userToUpdate.Username == value) return;
                _userToUpdate.Username = value;
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
        #endregion

        public UserUpdateViewModel(IUserService userService)
        {
            _userService = userService;
            UpdateUser = new RelayCommand(ExecuteUpdateUser);
            MessengerInstance.Register<UserUpdateRequest>(this, message =>
            {
                UserToUpdate = message.User;
            });
        }

        private void ExecuteUpdateUser()
        {
            UsernameAlreadyExists = false;
            NonMatchingPasswords = false;

            Task.Run(() =>
            {
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
