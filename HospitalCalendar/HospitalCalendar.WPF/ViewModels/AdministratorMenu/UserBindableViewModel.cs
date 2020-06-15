using System;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class UserBindableViewModel : ViewModelBase
    {
        public User User { get; set; }
        public bool IsSelected { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public UserBindableViewModel(User user)
        {
            User = user;
        }

        // Methods are used by Fody Weaver
        private void OnIsSelectedChanged()
        {
            MessengerInstance.Send(new UserBindableViewModelChanged());
        }
        private void OnUserChanged()
        {
            Username = User.Username;
            FirstName = User.FirstName;
            LastName = User.LastName;
        }
    }
}