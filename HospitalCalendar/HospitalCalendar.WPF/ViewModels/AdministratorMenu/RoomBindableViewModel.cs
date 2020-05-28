using System.ComponentModel;
using System.Runtime.CompilerServices;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{

    public class RoomBindableViewModel : ViewModelBase
    {
        #region Properties
        private bool _isSelected;
        private Room _room;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
                MessengerInstance.Send(new RoomBindableViewModelChanged());
            }
        }

        public string Number
        {
            get => _room.Number;
            set
            {
                if (_room.Number == value) return;
                _room.Number = value;
                RaisePropertyChanged(nameof(Number));
            }
        }

        public int Floor
        {
            get => _room.Floor;
            set
            {
                if (_room.Floor == value) return;
                _room.Floor = value;
                RaisePropertyChanged(nameof(Floor));
            }
        }

        public RoomType Type
        {
            get => _room.Type;
            set
            {
                if (_room.Type == value) return;
                _room.Type = value;
                RaisePropertyChanged(nameof(Type));
            }
        }

        public Room Room
        {
            get => _room;
            set
            {
                if (_room == value) return;
                _room = value;
                RaisePropertyChanged(nameof(Room));
            }
        }
        #endregion

        public RoomBindableViewModel(Room room)
        {
            Room = room;
        }
    }
}