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
        private readonly IEquipmentTypeService _equipmentTypeService;

        public ICommand CreateEquipmentType { get; set; }
        public IEnumerable<int> AmountEnumerable { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Amount { get; set; }
        public bool ValidationError { get; set; }
        public bool EquipmentTypeAlreadyExistsError { get; set; }

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