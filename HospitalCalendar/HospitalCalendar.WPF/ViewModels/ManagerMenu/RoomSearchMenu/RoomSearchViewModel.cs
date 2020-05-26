using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu
{
    public class RoomSearchViewModel : ViewModelBase
    {
        private readonly IRoomService _roomService;
        private readonly IEquipmentTypeService _equipmentTypeService;
        private readonly IEquipmentItemService _equipmentItemService;

        public DateTime? SearchStartDate { get; set; } = null;
        public DateTime? SearchEndDate { get; set; } = null;
        public DateTime? SearchStartTime { get; set; } = null;
        public DateTime? SearchEndTime { get; set; } = null;
        public bool SearchOccupiedRooms { get; set; } = false;
        public ICollection<EquipmentType> EquipmentTypesToSearchBy { get; set; } = new List<EquipmentType>();
        public ObservableCollection<EquipmentTypeBindableViewModel> AllEquipmentTypes { get; set; } = new ObservableCollection<EquipmentTypeBindableViewModel>();
        public ICommand Search { get; set; }
        public List<Room> SearchResults { get; set; }
        public ObservableCollection<RoomBindableViewModel> RoomBindableViewModels { get; set; } = new ObservableCollection<RoomBindableViewModel>();

        public RoomSearchViewModel(IRoomService roomService, IEquipmentTypeService equipmentTypeService, IEquipmentItemService equipmentItemService)
        {
            _roomService = roomService;
            _equipmentTypeService = equipmentTypeService;
            _equipmentItemService = equipmentItemService;
            Search = new RelayCommand(ExecuteSearch);
            MessengerInstance.Register<EquipmentTypeBindableViewModelChecked>(this, HandleEquipmentTypeBindableViewModelChanged);
        }

        private static void UiDispatch(Action action)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(action);
        }

        public void Initialize()
        {
            LoadEquipmentTypes();
        }

        private void LoadEquipmentTypes()
        {
            Task.Run(async () =>
            {
                var equipmentTypes = (await _equipmentTypeService.GetAll()).ToList();
                //equipmentTypes.Sort((x, y) => string.Compare(x.Name, y.Name, StringComparison.Ordinal));
                UiDispatch(() => AllEquipmentTypes.Clear());
                equipmentTypes.ForEach( et =>
                {
                    var amount = ( _equipmentItemService.GetAllByType(et)).GetAwaiter().GetResult().Count;
                    UiDispatch(() => AllEquipmentTypes.Add(new EquipmentTypeBindableViewModel(et, amount)));
                });
            });

        }

        private void HandleEquipmentTypeBindableViewModelChanged(EquipmentTypeBindableViewModelChecked obj)
        {
            EquipmentTypesToSearchBy = AllEquipmentTypes
                .Where(etbvm => etbvm.IsSelected)
                .Select(etbvm => etbvm.EquipmentType)
                .ToList();
        }

        private void ExecuteSearch()
        {
            Task.Run(async () =>
            {
                var searchStartPeriod = SearchStartDate + SearchStartTime?.TimeOfDay;
                var searchEndPeriod = SearchEndDate + SearchEndTime?.TimeOfDay;

                //if(SearchOccupiedRooms)
                // TODO: Will this actually include rooms within the query? test!
                if (searchStartPeriod != null && searchEndPeriod != null)
                {
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

                    UiDispatch(() => RoomBindableViewModels.Clear());
                    UiDispatch(() => SearchResults.ForEach(r => RoomBindableViewModels.Add(new RoomBindableViewModel(r))));
                }
            });
        }
    }
}