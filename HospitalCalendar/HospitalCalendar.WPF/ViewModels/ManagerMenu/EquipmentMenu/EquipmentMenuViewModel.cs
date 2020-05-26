using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentMenuViewModel : ViewModelBase
    {
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        public bool CanCreateEquipmentType { get; set; }
        public bool CanUpdateEquipmentType { get; set; }
        public int NumberOfEquipmentTypesChecked { get; set; }

        public ObservableCollection<EquipmentTypeBindableViewModel> AllEquipmentTypes { get; set; } = new ObservableCollection<EquipmentTypeBindableViewModel>();

        public EquipmentMenuViewModel(IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;

            MessengerInstance.Register<EquipmentTypeCreateSuccess>(this, HandleEquipmentTypeCreateSuccess);
            MessengerInstance.Register<EquipmentTypeUpdateSuccess>(this, HandleEquipmentTypeUpdateSuccess);
            MessengerInstance.Register<EquipmentTypeDeleteSuccess>(this, HandleEquipmentTypeDeleteSuccess);
            MessengerInstance.Register<EquipmentTypeBindableViewModelChecked>(this, HandleEquipmentTypeBindableViewModelChanged);
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        public void Initialize()
        {
            CanCreateEquipmentType = true;
            LoadEquipmentTypes();
        }

        private void LoadEquipmentTypes()
        {
            Task.Run(async () =>
            {
                var equipmentTypes = (await _equipmentTypeService.GetAll()).ToList();
                //equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                UiDispatch(() => AllEquipmentTypes.Clear());
                equipmentTypes.ForEach(async et =>
                {
                    var amount = (await _equipmentItemService.GetAllByType(et)).Count;
                    UiDispatch(() => AllEquipmentTypes.Add(new EquipmentTypeBindableViewModel(et, amount)));
                });
            });
        }

        private void HandleEquipmentTypeCreateSuccess(EquipmentTypeCreateSuccess message)
        {
            UiDispatch(() => AllEquipmentTypes.Insert(0, new EquipmentTypeBindableViewModel(message.EquipmentType, message.Amount)));
        }

        private void HandleEquipmentTypeUpdateSuccess(EquipmentTypeUpdateSuccess message)
        {
            UiDispatch(() =>
            {
                AllEquipmentTypes.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)
                    .EquipmentType = message.EquipmentType;

                AllEquipmentTypes.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)
                    .Amount = message.NewAmount;
            });
        }

        private void HandleEquipmentTypeDeleteSuccess(EquipmentTypeDeleteSuccess message)
        {
            UiDispatch(() => AllEquipmentTypes
                .Remove(AllEquipmentTypes
                    .First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)));
            CanCreateEquipmentType = true;
        }

        private void HandleEquipmentTypeBindableViewModelChanged(EquipmentTypeBindableViewModelChecked obj)
        {
            Task.Run(() =>
            {
                NumberOfEquipmentTypesChecked = AllEquipmentTypes.Count(etbvm => etbvm.IsSelected);

                switch (NumberOfEquipmentTypesChecked)
                {
                    case 0:
                        CanCreateEquipmentType = true;
                        CanUpdateEquipmentType = false;
                        break;
                    case 1:
                    {
                        CanCreateEquipmentType = false;
                        CanUpdateEquipmentType = true;

                        var selectedEquipmentType = AllEquipmentTypes.First(etbvm => etbvm.IsSelected);

                        MessengerInstance.Send(new EquipmentTypeSelected(selectedEquipmentType.EquipmentType, selectedEquipmentType.Amount));
                        break;
                    }
                    default:
                        CanCreateEquipmentType = false;
                        CanUpdateEquipmentType = false;
                        break;
                }
            });
        }
    }
}