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
    public class RoomCreateViewModel : ViewModelBase
    {
        private string _number;
        private int? _floor;
        private RoomType? _type;
        private bool _validationError;
        private bool _roomAlreadyExistsError;
        private readonly IRoomService _roomService;

        public ICommand CreateRoom { get; set; }

        public IEnumerable<RoomType> RoomTypes { get; set; }

        public IEnumerable<int> Floors { get; set; }

        public string Number
        {
            get => _number;
            set
            {
                if (_number == value) return;
                _number = value;
                RaisePropertyChanged(nameof(Number));
            }
        }

        public int? Floor
        {
            get => _floor;
            set
            {
                if (_floor == value) return;
                _floor = value;
                RaisePropertyChanged(nameof(Floor));
            }
        }

        public RoomType? Type
        {
            get => _type;
            set
            {
                if (_type == value) return;
                _type = value;
                RaisePropertyChanged(nameof(Type));
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

        public bool RoomAlreadyExistsError
        {
            get => _roomAlreadyExistsError;
            set
            {
                if (_roomAlreadyExistsError == value) return;
                _roomAlreadyExistsError = value;
                RaisePropertyChanged(nameof(RoomAlreadyExistsError));
            }
        }

        public RoomCreateViewModel(IRoomService roomService)
        {
            Type = null;
            _roomService = roomService;

            CreateRoom = new RelayCommand(ExecuteCreateRoom);

            Floors = Enumerable.Range(-2, 50);
            RoomTypes = Enum.GetValues(typeof(RoomType)).Cast<RoomType>().ToList();
        }

        private void ExecuteCreateRoom()
        {
            Task.Run(() =>
            {
                try
                {
                    ValidationError = false;
                    RoomAlreadyExistsError = false;

                    if (Type == null || string.IsNullOrWhiteSpace(Number) || Floor == null)
                    {
                        throw new ArgumentNullException();
                    }

                    var createdRoom = _roomService.Create(Floor.Value, Number.ToUpper(), Type.Value).GetAwaiter().GetResult();
                    MessengerInstance.Send(new RoomCreateSuccess(createdRoom));

                    Type = null;
                    Floor = null;
                    Number = "";
                }
                catch (ArgumentNullException)
                {
                    ValidationError = true;
                }
                catch (RoomAlreadyExistsException)
                {
                    RoomAlreadyExistsError = true;
                }
            });
        }
    }
}
