using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.WPF.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.WPF.ViewModels.AdministratorMenu
{
    public class AdministratorViewModel : ViewModelBase
    {
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;

        private bool _selectAllUsers;
        private bool _selectAllRooms;

        public int NumberOfUsersChecked { get; set; }
        public int NumberOfRoomsChecked { get; set; }
        public Administrator Administrator { get; set; }
        public ICommand DeleteUsers { get; set; }
        public ICommand DeleteRooms { get; set; }

        public bool CanRegisterUser { get; set; }
        public bool CanModifyUser { get; set; }
        public bool CanDeleteUser { get; set; }
        public bool CanCreateRoom { get; set; }

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

        public ObservableCollection<UserBindableViewModel> UserBindableViewModels { get; set; }
        public ObservableCollection<RoomBindableViewModel> RoomBindableViewModels { get; set; }

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

            MessengerInstance.Register<UserBindableViewModelChanged>(this, HandleUserBindableViewModelChanged);
            MessengerInstance.Register<RoomBindableViewModelChanged>(this, HandleRoomBindableViewModelChanged);
            MessengerInstance.Register<UserRegisterSuccess>(this, HandleUserRegisterSuccess);
            MessengerInstance.Register<RoomCreateSuccess>(this, HandleRoomCreateSuccess);
            MessengerInstance.Register<UserUpdateSuccess>(this, HandleUserUpdateSuccess);
            MessengerInstance.Register<CurrentUser>(this, message =>
            {
                Administrator = message.User as Administrator;
                LoadUsers();
                LoadRooms();
            });
        }

        private void HandleUserRegisterSuccess(UserRegisterSuccess message)
        {
            UserBindableViewModels.Insert(0, new UserBindableViewModel(message.User));
        }

        private void HandleRoomCreateSuccess(RoomCreateSuccess message)
        {
            RoomBindableViewModels.Add(new RoomBindableViewModel(message.Room));
            RoomBindableViewModels = new ObservableCollection<RoomBindableViewModel>(RoomBindableViewModels.OrderBy(rbvm => rbvm.Room.Floor));
        }

        private void HandleUserUpdateSuccess(UserUpdateSuccess message)
        {
            UserBindableViewModels.First(i => i.User.ID == message.User.ID).User = message.User;
        }

        private async void LoadRooms()
        {
            var rooms = (await _roomService.GetAll()).ToList();
            rooms.Sort((x, y) => x.Floor.CompareTo(y.Floor));
            rooms.ForEach(r => RoomBindableViewModels.Add(new RoomBindableViewModel(r)));
        }

        private async void LoadUsers()
        {
            var allUsers = await _userService.GetAll();
            var doctorsManagersSecretaries = allUsers.Where(u => u is Doctor || u is Manager || u is Secretary).ToList();
            doctorsManagersSecretaries.ForEach(u => UserBindableViewModels.Add(new UserBindableViewModel(u)));
        }

        private void ExecuteDeleteUsers()
        {
            UserBindableViewModels.Where(ubvm => ubvm.IsSelected)
                .ToList()
                .ForEach(ubvm =>
                {
                    _userService.Delete(ubvm.User.ID);
                    UserBindableViewModels.Remove(ubvm);
                });
            MessengerInstance.Send(new UserBindableViewModelChanged());
        }

        private void ExecuteDeleteRooms()
        {
            RoomBindableViewModels.Where(rbvm => rbvm.IsSelected)
                .ToList()
                .ForEach(rbvm =>
                {
                    _roomService.Delete(rbvm.Room.ID);
                    RoomBindableViewModels.Remove(rbvm);
                });
            MessengerInstance.Send(new RoomBindableViewModelChanged());
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