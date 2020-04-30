using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.EntityFramework.Exceptions;

namespace HospitalCalendar.EntityFramework.Services
{
    public class RoomService : GenericDataService<Room>, IRoomService
    {
        public RoomService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory) { }

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
                // TODO: Re-check

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
                return await context.Rooms
                                    .Where(r => r.IsActive)
                                    .Include(r => r.Equipment
                                        .Where(e => equipmentTypes
                                            .All(et => et.Name == e.EquipmentType.Name && e.IsActive)))
                                    .ToListAsync();
            }
        }

        public async Task<ICollection<Room>> GetAllOccupied(DateTime start, DateTime end)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.CalendarEntries
                                    .Where(ce => ce.StartDateTime >= start && ce.EndDateTime <= end)
                                    .Select(x => x.Room)
                                    .Where(r => r.IsActive)
                                    .ToListAsync();
            }

        }

        public async Task<ICollection<Room>> GetAllFree(DateTime start, DateTime end)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.CalendarEntries
                                    .Where(ce => ce.StartDateTime < start && ce.EndDateTime > end)
                                    .Where(r => r.IsActive)
                                    .Select(x => x.Room)
                                    .ToListAsync();
            }
        }

        public async Task<Room> Create(int floor, string number, RoomType type)
        {
            var existingRoom = GetByFloorAndNumber(floor, number).Result;

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
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Rooms
                                    .Where(r => r.IsActive)
                                    .Where(r => r.Type == roomType)
                                    .ToListAsync();
            }
        
        }

        public async Task<Room> AddItems(Room room, ICollection<EquipmentItem> equipment) 
        {
            foreach (var e in equipment)
            {
                room.Equipment.Add(e);
            }
            _ = await Update(room);

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