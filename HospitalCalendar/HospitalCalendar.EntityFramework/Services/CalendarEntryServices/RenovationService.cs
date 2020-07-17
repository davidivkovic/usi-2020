using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.EquipmentServices;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class RenovationService : GenericDataService<Renovation>, IRenovationService
    {
        private readonly IRoomService _roomService;
        private readonly IEquipmentItemService _equipmentItemService;

        public RenovationService(HospitalCalendarDbContextFactory contextFactory, IRoomService roomService, IEquipmentItemService equipmentItemService) : base(contextFactory)
        {
            _roomService = roomService;
            _equipmentItemService = equipmentItemService;
        }

        public RenovationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task Synchronize()
        {
            var renovations = await GetAll();
            foreach (var renovation in renovations)
            {
                await Execute(renovation);
            }
        }

        public new async Task<Renovation> Get(Guid id)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Renovations
                .Include(r => r.Room)
                    .ThenInclude(room => room.Equipment)
                        //.ThenInclude(e => e.EquipmentType)
                .Include(r => r.RoomToAdd)
                .Include(r => r.AddedEquipmentItems)
                    .ThenInclude(e => e.EquipmentType)
                .Include(r => r.RemovedEquipmentItems)
                    .ThenInclude(e => e.EquipmentType)
                .FirstOrDefaultAsync(r => r.IsActive && r.ID == id);
        }

        public async Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            var renovation = new Renovation
            {
                IsActive = true,
                Room = null,
                RoomToAdd = null,
                NewRoomType = newRoomType,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = splitting,
                RemovedEquipmentItems = null,
                AddedEquipmentItems = null
            };

            var createdRenovation = await Create(renovation);

            createdRenovation.Room = room;

            if(roomToAdd != null)
                createdRenovation.RoomToAdd = roomToAdd;

            var updatedRenovation = await Update(createdRenovation);

            if (addedEquipmentItems.Count > 0)
                updatedRenovation.AddedEquipmentItems = addedEquipmentItems;
            if (removedEquipmentItems.Count > 0)
                updatedRenovation.RemovedEquipmentItems = removedEquipmentItems;

            await Update(updatedRenovation, r => r.AddedEquipmentItems, r => r.RemovedEquipmentItems);

            return updatedRenovation;
        }

        public async Task<ICollection<Renovation>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Renovations
                .Include(r => r.Room)
                .Where(r => r.IsActive)
                .Where(r => (r.StartDateTime >= start && r.StartDateTime <= end) ||
                                    (r.EndDateTime >= start && r.EndDateTime <= end) ||
                                    (r.StartDateTime >= start && r.EndDateTime <= end))
                .ToListAsync();
        }

        #region Execute
        public async Task<bool> Execute(Renovation renovation)
        {
            if (renovation.Completed || renovation.EndDateTime > DateTime.Now)
            {
                return false;
            }

            renovation = await Get(renovation.ID);

            var roomToUpdate = await _roomService.Get(renovation.Room.ID);

            roomToUpdate.Type = renovation.NewRoomType;

            if (renovation.AddedEquipmentItems.Count > 0)
            {
                var itemsInRoom = roomToUpdate.Equipment.ToList();
                roomToUpdate.Equipment = new List<EquipmentItem>();
                await _roomService.Update(roomToUpdate, room => room.Equipment);

                foreach (var equipmentItem in renovation.AddedEquipmentItems.ToList())
                {
                    roomToUpdate.Equipment.Add(await _equipmentItemService.Get(equipmentItem.ID));
                }

                itemsInRoom.ForEach(ei => roomToUpdate.Equipment.Add(ei));

                await _roomService.Update(roomToUpdate, room => room.Equipment);

                foreach (var equipmentItem in renovation.AddedEquipmentItems)
                {
                    await _equipmentItemService.Create(equipmentItem.EquipmentType, 1);
                }
            }

            if (renovation.RemovedEquipmentItems.Count > 0)
            {
                var itemsInRoom = roomToUpdate.Equipment.ToList();
                roomToUpdate.Equipment = new List<EquipmentItem>();
                await _roomService.Update(roomToUpdate, room => room.Equipment);

                var idsToRemove = new HashSet<Guid>(renovation.RemovedEquipmentItems.Select(x => x.ID));

                foreach (var equipmentItem in itemsInRoom)
                {
                    roomToUpdate.Equipment.Add(await _equipmentItemService.Get(equipmentItem.ID));
                }

                roomToUpdate.Equipment = roomToUpdate.Equipment
                    .Where(equipmentItem => !idsToRemove.Contains(equipmentItem.ID))
                    .ToList();
                await _roomService.Update(roomToUpdate, room => room.Equipment);
            }

            if (renovation.Splitting)
            {
                var newRoom = new Room
                {
                    Floor = renovation.Room.Floor,
                    Type = renovation.NewRoomType,
                    IsActive = true,
                };

                if (!char.IsLetter(renovation.Room.Number.Last()))
                {
                    newRoom.Number = renovation.Room.Number + "B";
                    roomToUpdate.Number += "A";
                }
                else
                {
                    newRoom.Number = renovation.Room.Number + "2";
                    roomToUpdate.Number += "1";
                }
                await _roomService.Create(newRoom);
                await _roomService.Update(roomToUpdate);
            }

            else if (renovation.RoomToAdd != null)
            {
                renovation.RoomToAdd.IsActive = false;
                await _roomService.Update(renovation.RoomToAdd);
            }

            renovation = await Get(renovation.ID);
            renovation.Completed = true;
            await Update(renovation);
            return true;
        }
        #endregion
    }
}
