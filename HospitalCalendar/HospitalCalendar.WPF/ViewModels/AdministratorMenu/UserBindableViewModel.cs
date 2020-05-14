using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{

    public class UserBindableViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Properties
        private bool _isSelected;
        private User _user;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
                MessengerInstance.Send(new UserBindableViewModelChanged());
            }
        }

        public string Username
        {
            get => _user.Username;
            set
            {
                if (_user.Username == value) return;
                _user.Username = value;
                RaisePropertyChanged(nameof(Username));
            }
        }

        public string FirstName
        {
            get => _user.FirstName;
            set
            {
                if (_user.FirstName == value) return;
                _user.FirstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _user.LastName;
            set
            {
                if (_user.LastName == value) return;
                _user.LastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                RaisePropertyChanged(nameof(User));
                RaisePropertyChanged(nameof(Username));
                RaisePropertyChanged(nameof(FirstName));
                RaisePropertyChanged(nameof(LastName));
            }
        }
        #endregion

        public UserBindableViewModel(User user)
        {
            User = user;
            // TODO: Look into this, probably unnecessary
            /*
            MessengerInstance.Register<UserUpdateSuccess>(this, message =>
            {
                if (User.ID == message.User.ID)
                {
                    User = message.User;
                    IsSelected = true;
                }
            });*/
        }
    }
}