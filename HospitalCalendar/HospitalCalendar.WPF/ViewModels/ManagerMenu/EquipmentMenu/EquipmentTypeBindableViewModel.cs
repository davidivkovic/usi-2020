using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentTypeBindableViewModel : ViewModelBase
    {
        private bool _isSelected;

        public int Amount { get; set; }
        public EquipmentType EquipmentType { get; set; }

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
            get => EquipmentType.Name;
            set
            {
                if (EquipmentType.Name == value) return;
                EquipmentType.Name = value;
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(EquipmentType));
            }
        }

        public string Description
        {
            get => EquipmentType.Description;
            set
            {
                if (EquipmentType.Description == value) return;
                EquipmentType.Description = value;
                RaisePropertyChanged(nameof(Description));
                RaisePropertyChanged(nameof(EquipmentType));
            }
        }

        public EquipmentTypeBindableViewModel(EquipmentType equipmentType, int amount)
        {
            EquipmentType = equipmentType;
            Amount = amount;
        }
    }
}