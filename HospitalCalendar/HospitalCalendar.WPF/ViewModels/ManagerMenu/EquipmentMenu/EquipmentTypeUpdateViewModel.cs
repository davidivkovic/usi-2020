using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;
using PropertyChanged;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentTypeUpdateViewModel : ViewModelBase
    {
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
                RaisePropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => EquipmentTypeToUpdate?.Description;
            set
            {
                if (EquipmentTypeToUpdate.Description == value) return;
                EquipmentTypeToUpdate.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public int TotalAmount { get; set; }
        public int InUseAmount { get; set; }
        public int NewAmount { get; set; }
        public bool EquipmentTypeAlreadyExists { get; set; }
        public IEnumerable<int> AmountEnumerable { get; set; }
        public bool CanDeleteEquipmentType { get; set; }

        public EquipmentTypeUpdateViewModel(IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;

            UpdateEquipmentType = new RelayCommand(ExecuteUpdateEquipmentType);
            DeleteEquipmentType = new RelayCommand(ExecuteDeleteEquipmentType);

            MessengerInstance.Register<EquipmentTypeSelected>(this, HandleEquipmentTypeSelected);
        }

        private async void HandleEquipmentTypeSelected(EquipmentTypeSelected message)
        {
            EquipmentTypeToUpdate = message.EquipmentType;
            TotalAmount = message.Amount;
            NewAmount = TotalAmount;
            CanDeleteEquipmentType = false;

            InUseAmount = await _equipmentItemService.CountInUseByType(EquipmentTypeToUpdate);

            AmountEnumerable = Enumerable.Range(InUseAmount, 10000 - InUseAmount);
            if (InUseAmount == 0)
            {
                CanDeleteEquipmentType = true;
            }
        }

        private async void ExecuteUpdateEquipmentType()
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
        }

        private async Task TryToUpdateEquipmentType()
        {
            // Positive if the user is adding items, negative if user is removing items
            var amountDelta = NewAmount - TotalAmount;
            // Doesn't fire many times
            await _equipmentTypeService.Update(EquipmentTypeToUpdate, Name, Description, amountDelta);
            MessengerInstance.Send(new EquipmentTypeUpdateSuccess(EquipmentTypeToUpdate, NewAmount));
        }

        private async void ExecuteDeleteEquipmentType()
        {
            await _equipmentItemService.Remove(EquipmentTypeToUpdate, TotalAmount);
            await _equipmentTypeService.PhysicalDelete(EquipmentTypeToUpdate.ID);
            MessengerInstance.Send(new EquipmentTypeDeleteSuccess(EquipmentTypeToUpdate));
            MessengerInstance.Send(new EquipmentTypeBindableViewModelChecked());

        }
    }
}