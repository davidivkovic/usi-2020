using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu
{
    public class RoomSearchViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        public DateTime? SearchStartDate { get; set; }
        public DateTime? SearchEndDate { get; set; }
        public DateTime? SearchStartTime { get; set; }
        public DateTime? SearchEndTime { get; set; }
        public bool SearchOccupiedRooms { get; set; }
        public ICollection<EquipmentType> EquipmentTypesToSearchBy { get; set; } = new List<EquipmentType>();
        public ObservableCollection<EquipmentTypeBindableViewModel> AllEquipmentTypes { get; set; }
        public ICommand Search { get; set; }
        public List<Room> SearchResults { get; set; }
        public ObservableCollection<RoomBindableViewModel> RoomBindableViewModels { get; set; }

        public RoomSearchViewModel(IRoomService roomService, IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _roomService = roomService;
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;

            AllEquipmentTypes = new ObservableCollection<EquipmentTypeBindableViewModel>();
            RoomBindableViewModels = new ObservableCollection<RoomBindableViewModel>();
            Search = new RelayCommand(ExecuteSearch);
            MessengerInstance.Register<EquipmentTypeBindableViewModelChecked>(this, HandleEquipmentTypeBindableViewModelChanged);
        }

        public void Initialize()
        {
            SearchStartDate = SearchEndDate = SearchStartTime = SearchEndTime = null;
            SearchOccupiedRooms = false;
            LoadEquipmentTypes();
        }

        private async void LoadEquipmentTypes()
        {
            var equipmentTypes = (await _equipmentTypeService.GetAll()).ToList();
            equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
            AllEquipmentTypes.Clear();

            foreach (var equipmentType in equipmentTypes)
            {
                var amount = await _equipmentItemService.CountByType(equipmentType);
                AllEquipmentTypes.Add(new EquipmentTypeBindableViewModel(equipmentType, amount));
            }
        }

        private void HandleEquipmentTypeBindableViewModelChanged(EquipmentTypeBindableViewModelChecked message)
        {
            EquipmentTypesToSearchBy = AllEquipmentTypes
                .Where(etbvm => etbvm.IsSelected)
                .Select(etbvm => etbvm.EquipmentType)
                .ToList();
        }

        private async void ExecuteSearch()
        {
            var searchStartPeriod = SearchStartDate + SearchStartTime?.TimeOfDay;
            var searchEndPeriod = SearchEndDate + SearchEndTime?.TimeOfDay;

            if (searchStartPeriod == null || searchEndPeriod == null) return;

            IEnumerable<Room> searchResultsByDate;

            if (SearchOccupiedRooms)
            {
                searchResultsByDate = await _roomService.GetAllOccupied(searchStartPeriod.Value, searchEndPeriod.Value);
            }
            else
            {
                searchResultsByDate = await _roomService.GetAllFree(searchStartPeriod.Value, searchEndPeriod.Value);
            }

            var searchResultsByEquipmentTypes = await _roomService.GetAllByEquipmentTypes(EquipmentTypesToSearchBy);

            var searchResultIDs = searchResultsByDate
                .Select(r => r.ID)
                .Intersect(searchResultsByEquipmentTypes.Select(s => s.ID));

            SearchResults = searchResultsByDate
                .Where(r => searchResultIDs
                    .Contains(r.ID))
                .ToList();

            RoomBindableViewModels.Clear();
            SearchResults.ForEach(room => RoomBindableViewModels.Add(new RoomBindableViewModel(room)));
        }
    }
}