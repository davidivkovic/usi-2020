using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;
using System.Threading.Tasks;
using System.Windows.Input;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserUpdateViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        public ICommand UpdateUser { get; set; }
        public User UserToUpdate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
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
                FirstName = UserToUpdate.FirstName;
                LastName = UserToUpdate.LastName;
                Username = UserToUpdate.Username;
            });
        }

        private async void ExecuteUpdateUser()
        {
            UsernameAlreadyExists = false;
            NonMatchingPasswords = false;

            if (Password != ConfirmPassword)
            {
                NonMatchingPasswords = true;
                return;
            }

            try
            {
                var userToUpdate = await _userService.Get(UserToUpdate.ID);
                var updatedUser = await _userService.Update(userToUpdate, FirstName, LastName, Username, Password);
                MessengerInstance.Send(new UserUpdateSuccess(updatedUser));
            }
            catch (UsernameAlreadyExistsException)
            {
                UsernameAlreadyExists = true;
            }
        }
    }
}