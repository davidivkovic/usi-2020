using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class RenovationService : GenericDataService<Renovation>, IRenovationService
    {
        public RenovationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            var renovation = new Renovation()
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
            var updatedRenovation = await Update(createdRenovation, room, roomToAdd, newRoomType, start, end, splitting, removedEquipmentItems, addedEquipmentItems);
            return updatedRenovation;
        }

        public async Task<Renovation> Create(Room room, RoomType newRoomType, DateTime start, DateTime end, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            var renovation = new Renovation()
            {
                IsActive = true,
                Room = null,
                RoomToAdd = null,
                NewRoomType = newRoomType,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = false,
                RemovedEquipmentItems = null,
                AddedEquipmentItems = null
            };

            var createdRenovation = await Create(renovation);
            var updatedRenovation = await Update(createdRenovation, room, null, newRoomType, start, end, false, removedEquipmentItems, addedEquipmentItems);
            return updatedRenovation;
        }

        public async Task<Renovation> Create(Room room, DateTime start, DateTime end, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            var renovation = new Renovation()
            {
                IsActive = true,
                Room = null,
                RoomToAdd = null,
                NewRoomType = room.Type,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = false,
                RemovedEquipmentItems = null,
                AddedEquipmentItems = null
            };

            var createdRenovation = await Create(renovation);
            var updatedRenovation = await Update(createdRenovation, room, null, room.Type, start, end, false, removedEquipmentItems, addedEquipmentItems);
            return updatedRenovation;
        }

        public async Task<Renovation> Create(Room room, DateTime start, DateTime end, bool splitting)
        {
            var renovation = new Renovation
            {
                IsActive = true,
                Room = null,
                RoomToAdd = null,
                NewRoomType = room.Type,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = splitting,
                RemovedEquipmentItems = null,
                AddedEquipmentItems = null
            };

            var createdRenovation = await Create(renovation);
            var updatedRenovation = await Update(createdRenovation, room, null, room.Type, start, end, splitting, null, null);
            return updatedRenovation;
        }

        public async Task<Renovation> Create(Room room, Room roomToAdd, DateTime start, DateTime end)
        {
            var renovation = new Renovation
            {
                IsActive = true,
                Room = null,
                RoomToAdd = null,
                NewRoomType = room.Type,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = false,
                RemovedEquipmentItems = null,
                AddedEquipmentItems = null
            };

            var createdRenovation = await Create(renovation);
            var updatedRenovation = await Update(createdRenovation, room, roomToAdd, room.Type, start, end, false, null, null);
            return updatedRenovation;
        }

        public async Task<Renovation> Create(Room room, DateTime start, DateTime end)
        {
            var renovation = new Renovation
            {
                IsActive = true,
                Room = null,
                RoomToAdd = null,
                NewRoomType = room.Type,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = false,
                RemovedEquipmentItems = null,
                AddedEquipmentItems = null
            };

            var createdRenovation = await Create(renovation);
            var updatedRenovation = await Update(createdRenovation, room, null, room.Type, start, end, false, null, null);
            return updatedRenovation;
        }

        public async Task<Renovation> Update(Renovation renovation, Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            renovation.Room = room;
            renovation.RoomToAdd = roomToAdd;
            renovation.NewRoomType = newRoomType;
            renovation.StartDateTime = start;
            renovation.EndDateTime = end;
            renovation.Splitting = splitting;
            renovation.RemovedEquipmentItems = removedEquipmentItems;
            renovation.AddedEquipmentItems = addedEquipmentItems;

            return await Update(renovation);
        }


        public async Task<ICollection<Renovation>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();
            return await context.Renovations
                .Include(r => r.Room)
                .Where(r => r.IsActive)
                .Where(r => (r.StartDateTime >= start && r.StartDateTime <= end) ||
                                    (r.EndDateTime >= start && r.EndDateTime <= end) ||
                                    (r.StartDateTime >= start && r.EndDateTime <= end))
                .ToListAsync();
        }
    }
}
