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

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu
{
    public class ManagerMenuViewModel : ViewModelBase
    {
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
        }

        private void ExecuteShowEquipmentMenu()
        {
            //throw new NotImplementedException();
            CurrentViewModel = EquipmentMenuViewModel;
        }

        private void ExecuteShowRenovationMenu()
        {
            CurrentViewModel = EquipmentTypeUpdateViewModel;
            //throw new NotImplementedException();
        }

        private void ExecuteShowDoctorMenu()
        {
            throw new NotImplementedException();
        }
    }
}
