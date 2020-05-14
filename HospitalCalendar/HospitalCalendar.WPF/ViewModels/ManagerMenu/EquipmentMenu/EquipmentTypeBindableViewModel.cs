using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentTypeBindableViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private EquipmentType _equipmentType;
        private int _amount;
        private bool _isSelected;

        public EquipmentType EquipmentType
        {
            get => _equipmentType;
            set
            {
                _equipmentType = value;
                RaisePropertyChanged(nameof(EquipmentType));
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(Description));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                RaisePropertyChanged(nameof(IsSelected));
                MessengerInstance.Send(new EquipmentTypeBindableViewModelChecked());
            }
        }

        public string Name
        {
            get => _equipmentType.Name;
            set
            {
                if (_equipmentType.Name == value) return;
                _equipmentType.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _equipmentType.Description;
            set
            {
                if (_equipmentType.Description == value) return;
                _equipmentType.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public int Amount
        {
            get => _amount;
            set
            {
                if (_amount == value) return;
                _amount  = value;
                RaisePropertyChanged(nameof(Amount));
            }
        }

        public EquipmentTypeBindableViewModel(EquipmentType equipmentType, int amount)
        {
            EquipmentType = equipmentType;
            Amount = amount;
        }
    }
}
