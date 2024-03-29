﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.DoctorSpecializationsMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.ReportMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;
using System.Windows.Input;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu
{
    public class ManagerMenuViewModel : ViewModelBase
    {
        public ICommand ShowEquipmentMenu { get; set; }
        public ICommand ShowRenovationMenu { get; set; }
        public ICommand ShowDoctorMenu { get; set; }
        public ICommand ShowRoomSearchMenu { get; set; }
        public ICommand ShowReportMenu { get; set; }
        public Manager Manager { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }
        public EquipmentMenuViewModel EquipmentMenuViewModel { get; set; }
        public RenovationMenuViewModel RenovationMenuViewModel { get; set; }
        public RoomSearchViewModel RoomSearchViewModel { get; set; }
        public DoctorSpecializationsViewModel DoctorSpecializationsViewModel { get; set; }
        public ManagerReportMenuViewModel ManagerReportMenuViewModel { get; set; }

        public ManagerMenuViewModel(EquipmentMenuViewModel equipmentMenuViewModel, RenovationMenuViewModel renovationMenuViewModel, ManagerReportMenuViewModel managerReportMenuViewModel,
                                    RoomSearchViewModel roomSearchViewModel, DoctorSpecializationsViewModel doctorSpecializationsViewModel)
        {
            EquipmentMenuViewModel = equipmentMenuViewModel;
            RenovationMenuViewModel = renovationMenuViewModel;
            ManagerReportMenuViewModel = managerReportMenuViewModel;
            RoomSearchViewModel = roomSearchViewModel;
            DoctorSpecializationsViewModel = doctorSpecializationsViewModel;

            MessengerInstance.Register<CurrentUser>(this, message => Manager = message.User as Manager);
            ShowEquipmentMenu = new RelayCommand(ExecuteShowEquipmentMenu);
            ShowRenovationMenu = new RelayCommand(ExecuteShowRenovationMenu);
            ShowDoctorMenu = new RelayCommand(ExecuteShowDoctorMenu);
            ShowRoomSearchMenu = new RelayCommand(ExecuteShowRoomSearchMenu);
            ShowReportMenu = new RelayCommand(ExecuteShowReportMenu);

            ExecuteShowRenovationMenu();
        }

        private void ExecuteShowReportMenu()
        {
            if (CurrentViewModel == ManagerReportMenuViewModel) return;
            ManagerReportMenuViewModel.Initialize();
            CurrentViewModel = ManagerReportMenuViewModel;
        }

        private void ExecuteShowRoomSearchMenu()
        {
            if (CurrentViewModel == RoomSearchViewModel) return;
            RoomSearchViewModel.Initialize();
            CurrentViewModel = RoomSearchViewModel;
        }

        private void ExecuteShowEquipmentMenu()
        {
            if (CurrentViewModel == EquipmentMenuViewModel) return;
            EquipmentMenuViewModel.Initialize();
            CurrentViewModel = EquipmentMenuViewModel;
        }

        private void ExecuteShowRenovationMenu()
        {
            if (CurrentViewModel == RenovationMenuViewModel) return;
            RenovationMenuViewModel.Initialize();
            CurrentViewModel = RenovationMenuViewModel;
        }

        private void ExecuteShowDoctorMenu()
        {
            if (CurrentViewModel == DoctorSpecializationsViewModel) return;
            DoctorSpecializationsViewModel.Initialize();
            CurrentViewModel = DoctorSpecializationsViewModel;
        }
    }
}