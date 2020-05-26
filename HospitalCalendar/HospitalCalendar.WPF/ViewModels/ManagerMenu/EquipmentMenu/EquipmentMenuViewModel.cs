using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.Messages;
using System;
<<<<<<< HEAD
=======
using System.Collections.Generic;
>>>>>>> viewmodel-development
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
<<<<<<< HEAD
=======
using HospitalCalendar.Domain.Services;
>>>>>>> viewmodel-development

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu
{
    public class EquipmentMenuViewModel : ViewModelBase
    {
<<<<<<< HEAD
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
=======
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        public bool CanCreateEquipmentType { get; set; }
        public bool CanUpdateEquipmentType { get; set; }
        public int NumberOfEquipmentTypesChecked { get; set; }

        public ObservableCollection<EquipmentTypeBindableViewModel> AllEquipmentTypes { get; set; } = new ObservableCollection<EquipmentTypeBindableViewModel>();
>>>>>>> viewmodel-development

        public EquipmentMenuViewModel(IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;

<<<<<<< HEAD
            EquipmentTypeBindableViewModels = new ObservableCollection<EquipmentTypeBindableViewModel>();
            LoadEquipmentTypes();

=======
>>>>>>> viewmodel-development
            MessengerInstance.Register<EquipmentTypeCreateSuccess>(this, HandleEquipmentTypeCreateSuccess);
            MessengerInstance.Register<EquipmentTypeUpdateSuccess>(this, HandleEquipmentTypeUpdateSuccess);
            MessengerInstance.Register<EquipmentTypeDeleteSuccess>(this, HandleEquipmentTypeDeleteSuccess);
            MessengerInstance.Register<EquipmentTypeBindableViewModelChecked>(this, HandleEquipmentTypeBindableViewModelChanged);
<<<<<<< HEAD

            CanCreateEquipmentType = true;
=======
>>>>>>> viewmodel-development
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

<<<<<<< HEAD
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
=======
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
>>>>>>> viewmodel-development
                });
            });
        }

        private void HandleEquipmentTypeCreateSuccess(EquipmentTypeCreateSuccess message)
        {
<<<<<<< HEAD
            UiDispatch(() => EquipmentTypeBindableViewModels.Insert(0, new EquipmentTypeBindableViewModel(message.EquipmentType, message.Amount)));
=======
            UiDispatch(() => AllEquipmentTypes.Insert(0, new EquipmentTypeBindableViewModel(message.EquipmentType, message.Amount)));
>>>>>>> viewmodel-development
        }

        private void HandleEquipmentTypeUpdateSuccess(EquipmentTypeUpdateSuccess message)
        {
<<<<<<< HEAD
            UiDispatch(() => EquipmentTypeBindableViewModels.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID).EquipmentType = message.EquipmentType);
            UiDispatch(() => EquipmentTypeBindableViewModels.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID).Amount = message.NewAmount);
=======
            UiDispatch(() =>
            {
                AllEquipmentTypes.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)
                    .EquipmentType = message.EquipmentType;

                AllEquipmentTypes.First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)
                    .Amount = message.NewAmount;
            });
>>>>>>> viewmodel-development
        }

        private void HandleEquipmentTypeDeleteSuccess(EquipmentTypeDeleteSuccess message)
        {
<<<<<<< HEAD
            UiDispatch(() => EquipmentTypeBindableViewModels
                .Remove(EquipmentTypeBindableViewModels
=======
            UiDispatch(() => AllEquipmentTypes
                .Remove(AllEquipmentTypes
>>>>>>> viewmodel-development
                    .First(etbvm => etbvm.EquipmentType.ID == message.EquipmentType.ID)));
            CanCreateEquipmentType = true;
        }

        private void HandleEquipmentTypeBindableViewModelChanged(EquipmentTypeBindableViewModelChecked obj)
        {
<<<<<<< HEAD
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

=======
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
>>>>>>> viewmodel-development
