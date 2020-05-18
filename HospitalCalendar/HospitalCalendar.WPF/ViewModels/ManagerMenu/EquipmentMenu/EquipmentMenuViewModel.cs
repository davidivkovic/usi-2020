using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.Messages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentMenuViewModel : ViewModelBase
    {
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        public ObservableCollection<EquipmentTypeBindableViewModel> EquipmentTypeBindableViewModels { get; set; }
        public bool CanCreateEquipmentType { get; set; }
        public bool CanUpdateEquipmentType { get; set; }
        public int NumberOfEquipmentTypesChecked { get; set; }

        public EquipmentMenuViewModel(IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;

            EquipmentTypeBindableViewModels = new ObservableCollection<EquipmentTypeBindableViewModel>();
            LoadEquipmentTypes();

            MessengerInstance.Register<EquipmentTypeCreateSuccess>(this, HandleEquipmentTypeCreateSuccess);
            MessengerInstance.Register<EquipmentTypeUpdateSuccess>(this, HandleEquipmentTypeUpdateSuccess);
            MessengerInstance.Register<EquipmentTypeDeleteSuccess>(this, HandleEquipmentTypeDeleteSuccess);
            MessengerInstance.Register<EquipmentTypeBindableViewModelChecked>(this, HandleEquipmentTypeBindableViewModelChanged);

            CanCreateEquipmentType = true;
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        private void LoadEquipmentTypes()
        {
            Task.Run(async () =>
            {
                var equipmentTypes = (await _equipmentTypeService.GetAll()).ToList();
                equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));

                equipmentTypes.ForEach(async et =>
                {
                    int amount = (await _equipmentItemService.GetAllByType(et)).Count;
                    UiDispatch(() => EquipmentTypeBindableViewModels.Add(new EquipmentTypeBindableViewModel(et, amount)));
                });
            });
        }

        private void HandleEquipmentTypeCreateSuccess(EquipmentTypeCreateSuccess message)
        {
            UiDispatch(() => EquipmentTypeBindableViewModels.Insert(0, new EquipmentTypeBindableViewModel(message.EquipmentType, message.Amount)));
        }

        private void HandleEquipmentTypeUpdateSuccess(EquipmentTypeUpdateSuccess message)
        {
            UiDispatch(() => EquipmentTypeBindableViewModels.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID).EquipmentType = message.EquipmentType);
            UiDispatch(() => EquipmentTypeBindableViewModels.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID).Amount = message.NewAmount);
        }

        private void HandleEquipmentTypeDeleteSuccess(EquipmentTypeDeleteSuccess message)
        {
            UiDispatch(() => EquipmentTypeBindableViewModels
                .Remove(EquipmentTypeBindableViewModels
                    .First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)));
            CanCreateEquipmentType = true;
        }

        private void HandleEquipmentTypeBindableViewModelChanged(EquipmentTypeBindableViewModelChecked obj)
        {
            Task.Run(() =>
            {
                NumberOfEquipmentTypesChecked = EquipmentTypeBindableViewModels.Count(etbvm => etbvm.IsSelected);

                if (NumberOfEquipmentTypesChecked == 0)
                {
                    CanCreateEquipmentType = true;
                    CanUpdateEquipmentType = false;
                }
                else if (NumberOfEquipmentTypesChecked == 1)
                {
                    CanCreateEquipmentType = false;
                    CanUpdateEquipmentType = true;

                    var selectedEquipmentType = EquipmentTypeBindableViewModels
                        .First(etbvm => etbvm.IsSelected);

                    MessengerInstance.Send(new EquipmentTypeSelected(selectedEquipmentType.EquipmentType, selectedEquipmentType.Amount));
                }
                else
                {
                    CanCreateEquipmentType = false;
                    CanUpdateEquipmentType = false;
                }
            });
        }
    }
}