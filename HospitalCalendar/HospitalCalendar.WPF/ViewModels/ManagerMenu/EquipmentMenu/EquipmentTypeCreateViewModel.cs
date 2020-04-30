using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentTypeCreateViewModel : ViewModelBase
    {
        #region Properties
        private string _name;
        private string _description;
        private int? _amount;
        private bool _validationError;
        private bool _equipmentTypeAlreadyExistsError;
        private readonly IEquipmentTypeService _equipmentTypeService;

        public ICommand CreateEquipmentType { get; set; }

        public IEnumerable<int> AmountEnumerable { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public int? Amount
        {
            get => _amount;
            set
            {
                if (_amount == value) return;
                _amount = value;
                RaisePropertyChanged(nameof(Amount));
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

        public bool EquipmentTypeAlreadyExistsError
        {
            get => _equipmentTypeAlreadyExistsError;
            set
            {
                if (_equipmentTypeAlreadyExistsError == value) return;
                _equipmentTypeAlreadyExistsError = value;
                RaisePropertyChanged(nameof(EquipmentTypeAlreadyExistsError));
            }
        }
        #endregion

        public EquipmentTypeCreateViewModel(IEquipmentTypeService equipmentTypeService)
        {
            _equipmentTypeService = equipmentTypeService;
            CreateEquipmentType = new RelayCommand(ExecuteCreateEquipmentType);

            AmountEnumerable = Enumerable.Range(1, 10000);
        }

        private void ExecuteCreateEquipmentType()
        {
            Task.Run(async () =>
            {
                try
                {
                    ValidationError = false;
                    EquipmentTypeAlreadyExistsError = false;

                    if (string.IsNullOrWhiteSpace(Description) || string.IsNullOrWhiteSpace(Name) || Amount == null)
                    {
                        throw new ArgumentNullException();
                    }

                    var createdEquipmentType = await _equipmentTypeService.Create(Name, Description, Amount.Value);
                    MessengerInstance.Send(new EquipmentTypeCreateSuccess(createdEquipmentType, Amount.Value));

                    Name = null;
                    Description = null;
                    Amount = null;
                }
                catch (ArgumentNullException)
                {
                    ValidationError = true;
                }
                catch (EquipmentTypeAlreadyExistsException)
                {
                    EquipmentTypeAlreadyExistsError = true;
                }
            });
        }
    }
}
