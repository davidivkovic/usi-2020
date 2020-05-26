using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
<<<<<<< HEAD
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
=======
using HospitalCalendar.WPF.ViewModels.ManagerMenu.DoctorSpecializationsMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;
>>>>>>> viewmodel-development

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu
{
    public class ManagerMenuViewModel : ViewModelBase
    {
<<<<<<< HEAD
        #region Properties
        private ViewModelBase _currentViewModel;

        public ICommand ShowEquipmentMenu { get; set; }
        public ICommand ShowRenovationMenu { get; set; }
        public ICommand ShowDoctorMenu { get; set; }

        public Manager Manager { get; set; }

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;

            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged(nameof(CurrentViewModel));

            }
        }
        #endregion

        private EquipmentMenuViewModel _equipmentMenuViewModel;
        
        public EquipmentMenuViewModel EquipmentMenuViewModel
        {
            get => _equipmentMenuViewModel;

            set
            {
                if (_equipmentMenuViewModel == value)
                    return;
                _equipmentMenuViewModel = value;
                RaisePropertyChanged(nameof(EquipmentMenuViewModel));

            }
        }
        
        public EquipmentTypeUpdateViewModel EquipmentTypeUpdateViewModel { get; set; }

        public ManagerMenuViewModel(EquipmentMenuViewModel equipmentMenuViewModel, EquipmentTypeUpdateViewModel equipmentTypeUpdateViewModel)
        {
            EquipmentMenuViewModel = equipmentMenuViewModel;
            EquipmentTypeUpdateViewModel = equipmentTypeUpdateViewModel;

            ShowEquipmentMenu = new RelayCommand(ExecuteShowEquipmentMenu);
            ShowRenovationMenu = new RelayCommand(ExecuteShowRenovationMenu);
            ShowDoctorMenu = new RelayCommand(ExecuteShowDoctorMenu);

            ExecuteShowEquipmentMenu();

            MessengerInstance.Register<CurrentUser>(this, message => Manager = message.User as Manager);
=======
        public ICommand ShowEquipmentMenu { get; set; }
        public ICommand ShowRenovationMenu { get; set; }
        public ICommand ShowDoctorMenu { get; set; }
        public ICommand ShowRoomSearchMenu { get; set; }

        public Manager Manager { get; set; }

        public ViewModelBase CurrentViewModel { get; set; }
        public EquipmentMenuViewModel EquipmentMenuViewModel { get; set; }
        public RenovationMenuViewModel RenovationMenuViewModel { get; set; }
        public RoomSearchViewModel RoomSearchViewModel { get; set; }
        public DoctorSpecializationsViewModel DoctorSpecializationsViewModel { get; set; }

        public ManagerMenuViewModel(EquipmentMenuViewModel equipmentMenuViewModel, RenovationMenuViewModel renovationMenuViewModel, 
                                    RoomSearchViewModel roomSearchViewModel, DoctorSpecializationsViewModel doctorSpecializationsViewModel)
        {
            EquipmentMenuViewModel = equipmentMenuViewModel;
            RenovationMenuViewModel = renovationMenuViewModel;
            RoomSearchViewModel = roomSearchViewModel;
            DoctorSpecializationsViewModel = doctorSpecializationsViewModel;

            MessengerInstance.Register<CurrentUser>(this, message => Manager = message.User as Manager);
            ShowEquipmentMenu = new RelayCommand(ExecuteShowEquipmentMenu);
            ShowRenovationMenu = new RelayCommand(ExecuteShowRenovationMenu);
            ShowDoctorMenu = new RelayCommand(ExecuteShowDoctorMenu);
            ShowRoomSearchMenu = new RelayCommand(ExecuteShowRoomSearchMenu);

            ExecuteShowRenovationMenu();
        }

        private void ExecuteShowRoomSearchMenu()
        {
            if (CurrentViewModel == RoomSearchViewModel) return;
            RoomSearchViewModel.Initialize();
            CurrentViewModel = RoomSearchViewModel;
>>>>>>> viewmodel-development
        }

        private void ExecuteShowEquipmentMenu()
        {
<<<<<<< HEAD
            //throw new NotImplementedException();
=======
            if (CurrentViewModel == EquipmentMenuViewModel) return;
            EquipmentMenuViewModel.Initialize();
>>>>>>> viewmodel-development
            CurrentViewModel = EquipmentMenuViewModel;
        }

        private void ExecuteShowRenovationMenu()
        {
<<<<<<< HEAD
            CurrentViewModel = EquipmentTypeUpdateViewModel;
            //throw new NotImplementedException();
=======
            if (CurrentViewModel == RenovationMenuViewModel) return;
            RenovationMenuViewModel.Initialize();
            CurrentViewModel = RenovationMenuViewModel;
>>>>>>> viewmodel-development
        }

        private void ExecuteShowDoctorMenu()
        {
<<<<<<< HEAD
            throw new NotImplementedException();
        }
    }
}
=======
            if (CurrentViewModel == DoctorSpecializationsViewModel) return;
            DoctorSpecializationsViewModel.Initialize();
            CurrentViewModel = DoctorSpecializationsViewModel;
        }
    }
}
>>>>>>> viewmodel-development
