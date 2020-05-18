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
    public class UserUpdateViewModel : ViewModelBase
    {
        private readonly IUserService _userService;

        public ICommand UpdateUser { get; set; }

        [AlsoNotifyFor(nameof(FirstName), nameof(LastName), nameof(Username))]
        public User UserToUpdate { get; set; }

        public string FirstName
        {
            get => UserToUpdate?.FirstName;
            set
            {
                if (UserToUpdate.FirstName == value) return;
                UserToUpdate.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => UserToUpdate?.LastName;
            set
            {
                if (UserToUpdate.LastName == value) return;
                UserToUpdate.LastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string Username
        {
            get => UserToUpdate?.Username;
            set
            {
                if (UserToUpdate.Username == value) return;
                UserToUpdate.Username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool UsernameAlreadyExists { get; set; }
        public bool NonMatchingPasswords { get; set; }

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