using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class AdministratorViewModel : ViewModelBase
    {
        private bool _canRegisterUser;
        private bool _canModifyUser;
        private bool _canDeleteUser;
        private bool _selectAllUsers;

        private bool _canCreateRoom;
        private bool _selectAllRooms;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;

        public int NumberOfUsersChecked { get; set; }
        public int NumberOfRoomsChecked { get; set; }
        public Administrator Administrator { get; set; }
        public ObservableCollection<UserBindableViewModel> UserBindableViewModels { get; set; }
        public ObservableCollection<RoomBindableViewModel> RoomBindableViewModels { get; set; }
        public ICommand DeleteUsers { get; set; }
        public ICommand DeleteRooms { get; set; }

        public bool CanRegisterUser
        {
            get => _canRegisterUser;
            set
            {
                if (_canRegisterUser == value) return;
                _canRegisterUser = value;
                RaisePropertyChanged(nameof(CanRegisterUser));
            }
        }

        public bool CanModifyUser
        {
            get => _canModifyUser;
            set
            {
                if (_canModifyUser == value) return;
                _canModifyUser = value;
                RaisePropertyChanged(nameof(CanModifyUser));
            }
        }

        public bool CanDeleteUser
        {
            get => _canDeleteUser;
            set
            {
                if (_canDeleteUser == value) return;
                _canDeleteUser = value;
                RaisePropertyChanged(nameof(CanDeleteUser));
            }
        }

        public bool SelectAllUsers
        {
            get => _selectAllUsers;

            set
            {
                if (_selectAllUsers == value) return;
                _selectAllUsers = value;

                foreach (var model in UserBindableViewModels)
                {
                    model.IsSelected = value;
                }

                RaisePropertyChanged(nameof(SelectAllUsers));
            }
        }

        public bool CanCreateRoom
        {
            get => _canCreateRoom;
            set
            {
                if (_canCreateRoom == value) return;
                _canCreateRoom = value;
                RaisePropertyChanged(nameof(CanCreateRoom));
            }
        }

        public bool SelectAllRooms
        {
            get => _selectAllRooms;

            set
            {
                if (_selectAllRooms == value) return;
                _selectAllRooms = value;

                foreach (var model in RoomBindableViewModels)
                {
                    model.IsSelected = value;
                }

                RaisePropertyChanged(nameof(_selectAllRooms));
            }
        }

        public AdministratorViewModel(IUserService userService, IRoomService roomService)
        {
            _userService = userService;
            _roomService = roomService;

            CanRegisterUser = true;
            CanCreateRoom = true;

            LoadRooms();
            LoadUsers();

            DeleteUsers = new RelayCommand(ExecuteDeleteUsers);
            DeleteRooms = new RelayCommand(ExecuteDeleteRooms);
            MessengerInstance.Register<UserBindableViewModelChanged>(this, HandleUserBindableViewModelChanged);
            MessengerInstance.Register<RoomBindableViewModelChanged>(this, HandleRoomBindableViewModelChanged);

            MessengerInstance.Register<CurrentUser>(this, message =>
            {
                Administrator = message.User as Administrator;
            });

            MessengerInstance.Register<UserRegisterSuccess>(this, message =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    UserBindableViewModels.Insert(0, new UserBindableViewModel(message.User));
                });
            });

            MessengerInstance.Register<RoomCreateSuccess>(this, message =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    RoomBindableViewModels.Add(new RoomBindableViewModel(message.Room));
                    RoomBindableViewModels = new ObservableCollection<RoomBindableViewModel>(RoomBindableViewModels.OrderBy(rbvm => rbvm.Room.Floor));
                    RaisePropertyChanged(nameof(RoomBindableViewModels));
                });
            });

            MessengerInstance.Register<UserUpdateSuccess>(this, message =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    var viewModelToUpdate = UserBindableViewModels.FirstOrDefault(i => i.User.ID == message.User.ID);

                    if (viewModelToUpdate != null)
                    {
                        var indexOf = UserBindableViewModels.IndexOf(viewModelToUpdate);
                        UserBindableViewModels.Remove(viewModelToUpdate);
                        UserBindableViewModels.Insert(indexOf, viewModelToUpdate);
                    }
                });
            });
        }

        private void LoadRooms()
        {
            Task.Run(() =>
            {
                RoomBindableViewModels = new ObservableCollection<RoomBindableViewModel>();

                var rooms = _roomService.GetAll().Result.ToList();
                rooms.Sort((x, y) => x.Floor.CompareTo(y.Floor));

                rooms.ForEach(r =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(delegate
                        {
                            RoomBindableViewModels.Add(new RoomBindableViewModel(r));
                        });
                    });
            });
        }

        private void LoadUsers()
        {
            Task.Run(() =>
            {
                UserBindableViewModels = new ObservableCollection<UserBindableViewModel>();

                var users = _userService.GetAll()
                    .Result
                    .Where(u => u is Doctor || u is Manager || u is Secretary).ToList();

                users.ForEach(u =>
                {
                    System.Windows.Application.Current.Dispatcher.Invoke(delegate
                    {
                        UserBindableViewModels.Add(new UserBindableViewModel(u));
                    });
                });
            });
        }

        private void ExecuteDeleteUsers()
        {
            Task.Run(() =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    UserBindableViewModels
                        .Where(ubvm => ubvm.IsSelected)
                        .ToList()
                        .ForEach(ubvm =>
                        {
                            _userService.Delete(ubvm.User.ID);
                            UserBindableViewModels.Remove(ubvm);
                        });
                });
                MessengerInstance.Send(new UserBindableViewModelChanged());
            });
        }

        private void ExecuteDeleteRooms()
        {
            Task.Run(() =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(delegate
                {
                    RoomBindableViewModels
                        .Where(rbvm => rbvm.IsSelected)
                        .ToList()
                        .ForEach(rbvm =>
                        {
                            _roomService.Delete(rbvm.Room.ID);
                            RoomBindableViewModels.Remove(rbvm);
                        });
                });
                MessengerInstance.Send(new RoomBindableViewModelChanged());
            });
        }

        private void HandleUserBindableViewModelChanged(UserBindableViewModelChanged obj)
        {
            NumberOfUsersChecked = UserBindableViewModels.Count(ubvm => ubvm.IsSelected);

            switch (NumberOfUsersChecked)
            {
                case 0:
                    CanRegisterUser = true;
                    CanModifyUser = false;
                    CanDeleteUser = false;
                    SelectAllUsers = false;
                    break;

                case 1:
                    CanRegisterUser = false;
                    CanModifyUser = true;
                    CanDeleteUser = true;
                    var userToUpdate = UserBindableViewModels.FirstOrDefault(ubvm => ubvm.IsSelected)?.User;
                    MessengerInstance.Send(new UserUpdateRequest(userToUpdate));
                    break;

                default:
                    CanRegisterUser = false;
                    CanDeleteUser = true;
                    CanModifyUser = false;
                    break;
            }
        }

        private void HandleRoomBindableViewModelChanged(RoomBindableViewModelChanged obj)
        {
            NumberOfRoomsChecked = RoomBindableViewModels.Count(rbvm => rbvm.IsSelected);
            CanCreateRoom = NumberOfRoomsChecked == 0;
        }
    }
}
