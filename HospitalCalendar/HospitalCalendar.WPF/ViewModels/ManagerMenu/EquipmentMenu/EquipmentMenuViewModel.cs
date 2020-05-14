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
        #region Properties
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;
        private ObservableCollection<EquipmentTypeBindableViewModel> _equipmentTypeBindableViewModels;
        private bool _canCreateEquipmentType;
        private bool _canUpdateEquipmentType;
        private bool _allEquipmentTypesSelected;

        public ObservableCollection<EquipmentTypeBindableViewModel> EquipmentTypeBindableViewModels
        {
            get => _equipmentTypeBindableViewModels;
            set
            {
                if (_equipmentTypeBindableViewModels == value) return;
                _equipmentTypeBindableViewModels = value;
                RaisePropertyChanged(nameof(EquipmentTypeBindableViewModels));
            }
        }

        public bool CanCreateEquipmentType
        {
            get => _canCreateEquipmentType;
            set
            {
                if (_canCreateEquipmentType == value) return;
                _canCreateEquipmentType = value;
                RaisePropertyChanged(nameof(CanCreateEquipmentType));
            }
        }

        public bool CanUpdateEquipmentType
        {
            get => _canUpdateEquipmentType;
            set
            {
                if (_canUpdateEquipmentType == value) return;
                _canUpdateEquipmentType = value;
                RaisePropertyChanged(nameof(CanUpdateEquipmentType));
            }
        }

        public bool AllEquipmentTypesSelected
        {
            get => _allEquipmentTypesSelected;
            set
            {
                if (_allEquipmentTypesSelected == value) return;
                _allEquipmentTypesSelected = value;

                foreach (var model in EquipmentTypeBindableViewModels)
                {
                    model.IsSelected = value;
                }

                RaisePropertyChanged(nameof(AllEquipmentTypesSelected));
            }
        }

        public int NumberOfEquipmentTypesChecked { get; set; }
        #endregion

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
            Task.Run(() =>
            {
                var equipmentTypes = _equipmentTypeService.GetAll().Result.ToList();
                equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));

                equipmentTypes.ForEach(et =>
                {
                    int amount = _equipmentItemService.GetAllByType(et).Result.ToList().Count;
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

                var equipmentTypeToUpdate = EquipmentTypeBindableViewModels
                    .First(etbvm => etbvm.IsSelected)
                    .EquipmentType;
                var equipmentTypeAmount = EquipmentTypeBindableViewModels
                    .First(etbvm => etbvm.IsSelected)
                    .Amount;

                MessengerInstance.Send(new EquipmentTypeSelected(equipmentTypeToUpdate, equipmentTypeAmount));
            }
            else
            {
                CanCreateEquipmentType = false;
                CanUpdateEquipmentType = false;
            }
        }
    }
}

