using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{

    public class UserBindableViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private bool _isSelected;
        private User _user;

        public new event PropertyChangedEventHandler PropertyChanged;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public string Username
        {
            get => User.Username;
            set
            {
                if (User.Username == value) return;
                User.Username = value;
                OnPropertyChanged();
            }
        }

        public string FirstName
        {
            get => User.FirstName;
            set
            {
                if (User.FirstName == value) return;
                User.FirstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => User.LastName;
            set
            {
                if (User.LastName == value) return;
                User.LastName = value;
                OnPropertyChanged();
            }
        }

        public User User
        {
            get => _user;
            set
            {
                if (_user == value) return;
                _user = value;
                OnPropertyChanged();
            }
        }

        public UserBindableViewModel(User user)
        {
            User = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Username = user.Username;
            MessengerInstance.Register<UserUpdateSuccess>(this, message =>
            {
                if (User.ID == message.User.ID)
                {
                    User = message.User;
                    IsSelected = true;
                }
            });
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            MessengerInstance.Send(new UserBindableViewModelChanged());
        }
    }
}