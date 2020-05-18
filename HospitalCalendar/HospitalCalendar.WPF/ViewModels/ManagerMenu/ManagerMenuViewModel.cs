using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu
{
    public class ManagerMenuViewModel : ViewModelBase
    {
        public ICommand ShowEquipmentMenu { get; set; }
        public ICommand ShowRenovationMenu { get; set; }
        public ICommand ShowDoctorMenu { get; set; }
        public ICommand ShowRoomSearchMenu { get; set; }

        public Manager Manager { get; set; }

        public ViewModelBase CurrentViewModel { get; set; }
        public EquipmentMenuViewModel EquipmentMenuViewModel { get; set; }
        public RenovationMenuViewModel RenovationMenuViewModel { get; set; }
        public RoomSearchViewModel RoomSearchViewModel { get; set; }


        public ManagerMenuViewModel(EquipmentMenuViewModel equipmentMenuViewModel, RenovationMenuViewModel renovationMenuViewModel, RoomSearchViewModel roomSearchViewModel)
        {
            EquipmentMenuViewModel = equipmentMenuViewModel;
            RenovationMenuViewModel = renovationMenuViewModel;
            RoomSearchViewModel = roomSearchViewModel;

            ShowEquipmentMenu = new RelayCommand(ExecuteShowEquipmentMenu);
            ShowRenovationMenu = new RelayCommand(ExecuteShowRenovationMenu);
            ShowDoctorMenu = new RelayCommand(ExecuteShowDoctorMenu);
            ShowRoomSearchMenu = new RelayCommand(ExecuteShowRoomSearchMenu);

            ExecuteShowEquipmentMenu();

            MessengerInstance.Register<CurrentUser>(this, message => Manager = message.User as Manager);
        }

        private void ExecuteShowRoomSearchMenu() => CurrentViewModel = RoomSearchViewModel;
        private void ExecuteShowEquipmentMenu() => CurrentViewModel = EquipmentMenuViewModel;
        private void ExecuteShowRenovationMenu() => CurrentViewModel = RenovationMenuViewModel;

        private void ExecuteShowDoctorMenu()
        {
            throw new NotImplementedException();
        }


    }
}
