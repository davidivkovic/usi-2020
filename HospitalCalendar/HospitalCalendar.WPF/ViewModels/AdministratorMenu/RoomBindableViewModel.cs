using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{

    public class RoomBindableViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private bool _isSelected;
        private Room _room;

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

        public string Number
        {
            get => Room.Number;
            set
            {
                if (Room.Number == value) return;
                Room.Number = value;
                OnPropertyChanged();
            }
        }

        public int Floor
        {
            get => Room.Floor;
            set
            {
                if (Room.Floor == value) return;
                Room.Floor = value;
                OnPropertyChanged();
            }
        }

        public RoomType Type
        {
            get => Room.Type;
            set
            {
                if (Room.Type == value) return;
                Room.Type = value;
                OnPropertyChanged();
            }
        }

        public Room Room
        {
            get => _room;
            set
            {
                if (_room == value) return;
                _room = value;
                OnPropertyChanged();
            }
        }

        public RoomBindableViewModel(Room room)
        {
            Room = room;
            Floor = room.Floor;
            Number = room.Number;
            Type = Room.Type;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            MessengerInstance.Send(new RoomBindableViewModelChanged());
        }
    }
}