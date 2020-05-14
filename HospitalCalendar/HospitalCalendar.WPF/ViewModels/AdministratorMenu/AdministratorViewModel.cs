using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class AdministratorViewModel : ViewModelBase
    {
        #region Properties
        private bool _canRegisterUser;
        private bool _canModifyUser;
        private bool _canDeleteUser;
        private bool _selectAllUsers;
        private bool _canCreateRoom;

        private bool _selectAllRooms;

        private ObservableCollection<UserBindableViewModel> _userBindableViewModels;
        private ObservableCollection<RoomBindableViewModel> _roomBindableViewModels;
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;

        public int NumberOfUsersChecked { get; set; }
        public int NumberOfRoomsChecked { get; set; }
        public Administrator Administrator { get; set; }
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

        public ObservableCollection<UserBindableViewModel> UserBindableViewModels
        {
            get => _userBindableViewModels;
            set
            {
                if (_userBindableViewModels == value) return;
                _userBindableViewModels = value;
                RaisePropertyChanged(nameof(UserBindableViewModels));
            }
        }

        public ObservableCollection<RoomBindableViewModel> RoomBindableViewModels
        {
            get => _roomBindableViewModels;
            set
            {
                if (_roomBindableViewModels == value) return;
                _roomBindableViewModels = value;
                RaisePropertyChanged(nameof(RoomBindableViewModels));
            }
        }
        #endregion

        public AdministratorViewModel(IUserService userService, IRoomService roomService)
        {
            _userService = userService;
            _roomService = roomService;

            DeleteUsers = new RelayCommand(ExecuteDeleteUsers);
            DeleteRooms = new RelayCommand(ExecuteDeleteRooms);
            CanRegisterUser = true;
            CanCreateRoom = true;

            UserBindableViewModels = new ObservableCollection<UserBindableViewModel>();
            RoomBindableViewModels = new ObservableCollection<RoomBindableViewModel>();

            LoadRooms();
            LoadUsers();

            MessengerInstance.Register<UserBindableViewModelChanged>(this, HandleUserBindableViewModelChanged);
            MessengerInstance.Register<RoomBindableViewModelChanged>(this, HandleRoomBindableViewModelChanged);
            MessengerInstance.Register<UserRegisterSuccess>(this, HandleUserRegisterSuccess);
            MessengerInstance.Register<RoomCreateSuccess>(this, HandleRoomCreateSuccess);
            MessengerInstance.Register<UserUpdateSuccess>(this, HandleUserUpdateSuccess);
            MessengerInstance.Register<CurrentUser>(this, message => Administrator = message.User as Administrator);
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        private void HandleUserRegisterSuccess(UserRegisterSuccess message)
        {
            UiDispatch(() => UserBindableViewModels.Insert(0, new UserBindableViewModel(message.User)));
        }

        private void HandleRoomCreateSuccess(RoomCreateSuccess message)
        {
            UiDispatch(() =>
            {
                RoomBindableViewModels.Add(new RoomBindableViewModel(message.Room));
                RoomBindableViewModels = new ObservableCollection<RoomBindableViewModel>(RoomBindableViewModels.OrderBy(rbvm => rbvm.Room.Floor));
            });
        }

        private void HandleUserUpdateSuccess(UserUpdateSuccess message)
        {
            UiDispatch(() => UserBindableViewModels.First(i => i.User.ID == message.User.ID).User = message.User);
        }

        private void LoadRooms()
        {
            Task.Run(() =>
            {
                var rooms = _roomService.GetAll().Result.ToList();
                rooms.Sort((x, y) => x.Floor.CompareTo(y.Floor));

                UiDispatch(() => rooms.ForEach(r => RoomBindableViewModels.Add(new RoomBindableViewModel(r))));
            });
        }

        private void LoadUsers()
        {
            Task.Run(async() =>
            {
                var allUsers = await _userService.GetAll();
                var doctorsManagersSecretaries = allUsers.Where(u => u is Doctor || u is Manager || u is Secretary).ToList();

                UiDispatch(() => doctorsManagersSecretaries.ForEach(u => UserBindableViewModels.Add(new UserBindableViewModel(u))));
            });
        }

        private void ExecuteDeleteUsers()
        {
            Task.Run(() =>
            {
                UiDispatch(() => UserBindableViewModels.Where(ubvm => ubvm.IsSelected)
                                .ToList()
                                .ForEach(ubvm =>
                                {
                                    _userService.Delete(ubvm.User.ID);
                                    UserBindableViewModels.Remove(ubvm);
                                }));
                MessengerInstance.Send(new UserBindableViewModelChanged());
            });
        }

        private void ExecuteDeleteRooms()
        {
            Task.Run(() =>
            {
                UiDispatch(() => RoomBindableViewModels.Where(rbvm => rbvm.IsSelected)
                        .ToList()
                        .ForEach(rbvm =>
                        {
                            _roomService.Delete(rbvm.Room.ID);
                            RoomBindableViewModels.Remove(rbvm);
                        }));
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
            //TODO: Fix "select all" not unchecking when deleting all rooms
            NumberOfRoomsChecked = RoomBindableViewModels.Count(rbvm => rbvm.IsSelected);
            CanCreateRoom = NumberOfRoomsChecked == 0;
        }
    }
}
