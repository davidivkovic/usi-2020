using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Exceptions;
using HospitalCalendar.WPF.Messages;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
<<<<<<< HEAD
=======
using PropertyChanged;
>>>>>>> viewmodel-development

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentTypeUpdateViewModel : ViewModelBase
    {
<<<<<<< HEAD
        #region Properties
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        private EquipmentType _equipmentTypeToUpdate;
        private int _totalAmount;
        private int _inUseAmount;
        private int _newAmount;
        private bool _equipmentTypeAlreadyExists;
        private IEnumerable<int> _amountEnumerable;
        private bool _canDeleteEquipmentType;

        public ICommand UpdateEquipmentType { get; set; }
        public ICommand DeleteEquipmentType { get; set; }

        public EquipmentType EquipmentTypeToUpdate
        {
            get => _equipmentTypeToUpdate;
            set
            {
                if (_equipmentTypeToUpdate == value) return;
                _equipmentTypeToUpdate = value;
                RaisePropertyChanged(nameof(EquipmentTypeToUpdate));
                RaisePropertyChanged(nameof(Name));
                RaisePropertyChanged(nameof(Description));
            }
        }

        public string Name
        {
            get => _equipmentTypeToUpdate?.Name;
            set
            {
                if (_equipmentTypeToUpdate.Name == value) return;
                _equipmentTypeToUpdate.Name = value;
=======
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        public ICommand UpdateEquipmentType { get; set; }
        public ICommand DeleteEquipmentType { get; set; }

        [AlsoNotifyFor(nameof(Name), nameof(Description))]
        public EquipmentType EquipmentTypeToUpdate { get; set; }

        public string Name
        {
            get => EquipmentTypeToUpdate?.Name;
            set
            {
                if (EquipmentTypeToUpdate.Name == value) return;
                EquipmentTypeToUpdate.Name = value;
>>>>>>> viewmodel-development
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
<<<<<<< HEAD
            get => _equipmentTypeToUpdate?.Description;
            set
            {
                if (_equipmentTypeToUpdate.Description == value) return;
                _equipmentTypeToUpdate.Description = value;
=======
            get => EquipmentTypeToUpdate?.Description;
            set
            {
                if (EquipmentTypeToUpdate.Description == value) return;
                EquipmentTypeToUpdate.Description = value;
>>>>>>> viewmodel-development
                RaisePropertyChanged(nameof(Description));
            }
        }

<<<<<<< HEAD
        public int TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount == value) return;
                _totalAmount = value;
                RaisePropertyChanged(nameof(TotalAmount));
            }
        }

        public int InUseAmount
        {
            get => _inUseAmount;
            set
            {
                if (_inUseAmount == value) return;
                _inUseAmount = value;
                RaisePropertyChanged(nameof(InUseAmount));
            }
        }

        public int NewAmount
        {
            get => _newAmount;
            set
            {
                if (_newAmount == value) return;
                _newAmount = value;
                RaisePropertyChanged(nameof(NewAmount));
            }
        }

        public bool EquipmentTypeAlreadyExists
        {
            get => _equipmentTypeAlreadyExists;
            set
            {
                if (_equipmentTypeAlreadyExists == value) return;
                _equipmentTypeAlreadyExists = value;
                RaisePropertyChanged(nameof(EquipmentTypeAlreadyExists));
            }
        }

        public IEnumerable<int> AmountEnumerable
        {
            get => _amountEnumerable;
            set
            {
                if (_amountEnumerable == value) return;
                _amountEnumerable = value;
                RaisePropertyChanged(nameof(AmountEnumerable));
            }
        }

        public bool CanDeleteEquipmentType
        {
            get => _canDeleteEquipmentType;
            set
            {
                if (_canDeleteEquipmentType == value) return;
                _canDeleteEquipmentType = value;
                RaisePropertyChanged(nameof(CanDeleteEquipmentType));
            }
        }
        #endregion
=======
        public int TotalAmount { get; set; }
        public int InUseAmount { get; set; }
        public int NewAmount { get; set; }
        public bool EquipmentTypeAlreadyExists { get; set; }
        public IEnumerable<int> AmountEnumerable { get; set; }
        public bool CanDeleteEquipmentType { get; set; }
>>>>>>> viewmodel-development

        public EquipmentTypeUpdateViewModel(IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;

            UpdateEquipmentType = new RelayCommand(ExecuteUpdateEquipmentType);
            DeleteEquipmentType = new RelayCommand(ExecuteDeleteEquipmentType);

            MessengerInstance.Register<EquipmentTypeSelected>(this, HandleEquipmentTypeSelected);
        }

<<<<<<< HEAD

        private void HandleEquipmentTypeSelected(EquipmentTypeSelected message)
        {
            EquipmentTypeToUpdate = message.EquipmentType;
            TotalAmount = message.Amount;
            NewAmount = TotalAmount;

            CanDeleteEquipmentType = false;

            Task.Run(async() =>
            {
                var equipmentItemsInUse = await _equipmentItemService.GetAllInUseByType(EquipmentTypeToUpdate);
                    InUseAmount = equipmentItemsInUse?.Count ?? 0;
                    AmountEnumerable = Enumerable.Range(InUseAmount, 10000 - InUseAmount);
                    if (InUseAmount == 0)
                    {
                        CanDeleteEquipmentType = true;
                    }
=======
        // This method needs refactoring - low maintainability
        private void HandleEquipmentTypeSelected(EquipmentTypeSelected message)
        {
            Task.Run(async () =>
            {
                EquipmentTypeToUpdate = message.EquipmentType;
                TotalAmount = message.Amount;
                NewAmount = TotalAmount;

                CanDeleteEquipmentType = false;

                InUseAmount = (await _equipmentItemService.GetAllInUseByType(EquipmentTypeToUpdate)).Count;
                //InUseAmount = 1;
                AmountEnumerable = Enumerable.Range(InUseAmount, 10000 - InUseAmount);
                if (InUseAmount == 0)
                {
                    CanDeleteEquipmentType = true;
                }
>>>>>>> viewmodel-development
            });
        }

        private void ExecuteUpdateEquipmentType()
        {
<<<<<<< HEAD
            Task.Run(async() =>
=======
            Task.Run(async () =>
>>>>>>> viewmodel-development
            {
                try
                {
                    EquipmentTypeAlreadyExists = false;
                    await TryToUpdateEquipmentType();
                }
                catch (EquipmentTypeAlreadyExistsException)
                {
                    EquipmentTypeAlreadyExists = true;
                }
            });
        }

        private async Task TryToUpdateEquipmentType()
        {
            // Positive if the user is adding items, negative if user is removing items
            int amountDelta = NewAmount - TotalAmount;
<<<<<<< HEAD
            var updatedEquipmentType = await _equipmentTypeService.Update(EquipmentTypeToUpdate, Name, Description, amountDelta);
            MessengerInstance.Send(new EquipmentTypeUpdateSuccess(updatedEquipmentType, NewAmount));
=======
            // Doesn't fire many times
            await _equipmentTypeService.Update(EquipmentTypeToUpdate, Name, Description, amountDelta);
            MessengerInstance.Send(new EquipmentTypeUpdateSuccess(new EquipmentType { Name = Name, Description = Description, IsActive = true, ID = EquipmentTypeToUpdate.ID }, NewAmount));
>>>>>>> viewmodel-development
        }

        private void ExecuteDeleteEquipmentType()
        {
            Task.Run(async () =>
            {
                await _equipmentItemService.Remove(EquipmentTypeToUpdate, TotalAmount);
                await _equipmentTypeService.PhysicalDelete(EquipmentTypeToUpdate.ID);
                MessengerInstance.Send(new EquipmentTypeDeleteSuccess(EquipmentTypeToUpdate));
<<<<<<< HEAD
            });
        }
    }
}
=======
                MessengerInstance.Send(new EquipmentTypeBindableViewModelChecked());
            });
        }
    }
}
>>>>>>> viewmodel-development
