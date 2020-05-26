using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.EntityFramework.Exceptions;

namespace HospitalCalendar.EntityFramework.Services
{
    public class RoomService : GenericDataService<Room>, IRoomService
    {
        private readonly ICalendarEntryService _calendarEntryService;

        public RoomService(HospitalCalendarDbContextFactory contextFactory, ICalendarEntryService calendarEntryService) : base(contextFactory)
        {
            _calendarEntryService = calendarEntryService;
        }

        public async Task<ICollection<Room>> GetAllByFloor(int floor)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Rooms
                    .Where(r => r.Floor == floor)
                    .Where(r => r.IsActive)
                    .ToListAsync();
            }
        }

        public async Task<Room> GetByFloorAndNumber(int floor, string number)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Rooms
                    .Where(r => r.Number == number)
                    .Where(r => r.Floor == floor)
                    .Where(r => r.IsActive)
                    .FirstOrDefaultAsync();
            }
        }

        public async Task<ICollection<Room>> GetAllByEquipmentType(EquipmentType equipmentType)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                // TODO: Re-check - doesnt work

                return await context.Rooms
                                    .Where(r => r.IsActive)
                                    .Include(r => r.Equipment
                                        .Select(e => e.EquipmentType.Name == equipmentType.Name && e.IsActive))
                                    .ToListAsync();
            }
        }

        public async Task<ICollection<Room>> GetAllByEquipmentTypes(ICollection<EquipmentType> equipmentTypes)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                var allRooms = await context.Rooms
                    .Where(r => r.IsActive)
                    .Include(r => r.Equipment)
                    .ThenInclude(e => e.EquipmentType)
                    .ToListAsync();

                if (equipmentTypes.Count == 0)
                {
                    return allRooms.Where(r => r.Equipment
                            .All(ei => equipmentTypes
                                .Any(et => et.Name == ei.EquipmentType.Name)))
                        .ToList();
                }

                return allRooms.Where(r => r.Equipment
                    .Any(ei => equipmentTypes
                        .All(et => et.Name == ei.EquipmentType.Name)))
                    .ToList();
            }
        }

        public async Task<ICollection<Room>> GetAllOccupied(DateTime start, DateTime end)
        {
            await using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                var f = await context.CalendarEntries
                    .Include(ce => ce.Room)
                    .Where(ce => ce.StartDateTime >= start && ce.EndDateTime <= end)
                    .Select(ce => ce.Room)
                    .Where(r => r.IsActive)
                    .ToListAsync();

                return f.GroupBy(r => r.ID)
                        .Select(g => g.First())
                        .ToList();
            }
        }

        public async Task<ICollection<Room>> GetAllFree(DateTime start, DateTime end)
        {
            var allRooms = await GetAll();
            var occupiedRooms = await GetAllOccupied(start, end);

            return await Task.Run(() =>
            {
                var freeRooms = allRooms
                    .Where(r => occupiedRooms
                        .All(or => or.ID != r.ID))
                    .ToList();
                return freeRooms;
            });
        }

        public async Task<Room> Create(int floor, string number, RoomType type)
        {
            var existingRoom = await GetByFloorAndNumber(floor, number);

            if (existingRoom != null)
            {
                throw new RoomAlreadyExistsException(floor, number);
            }

            Room created = new Room()
            {
                Floor = floor,
                Number = number,
                Type = type,
                Equipment = new List<EquipmentItem>(),
                IsActive = true
            };

            return await Create(created);
        }

        public new async Task<bool> Delete(Guid id)
        {
            var room = await Get(id);
            // TODO: Test removal of items from room
            room.Equipment?.Clear();
            room.IsActive = false;

            _ = await Update(room);

            return true;
        }

        public async Task<ICollection<Room>> GetAllByRoomType(RoomType roomType) 
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Rooms
                                    .Where(r => r.IsActive)
                                    .Where(r => r.Type == roomType)
                                    .ToListAsync();
            }
        
        }

        public async Task<Room> AddItems(Room room, ICollection<EquipmentItem> equipment)
        {
            //TODO: Check
            room.Equipment = equipment;
            /*
            foreach (var e in equipment)
            {
                room.Equipment.Add(e);
            }*/
            await Update(room);

            return room;
        
        }

        public async Task<Room> Update(Room entity, int floor, string number, RoomType roomType, ICollection<EquipmentItem> equipment) 
        {
            entity.Floor = floor;
            entity.Number = number;
            entity.Type = roomType;
            entity.Equipment = equipment;

            _ = await Update(entity);
            return entity;
        }
    }
}