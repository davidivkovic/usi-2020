﻿using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.EntityFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.EntityFramework.Services
{
    public class RoomService : GenericDataService<Room>, IRoomService
    {
        private readonly IDoctorService _doctorService;

        public RoomService(HospitalCalendarDbContextFactory contextFactory, IDoctorService doctorService) : base(contextFactory)
        {
            _doctorService = doctorService;
        }

        public new async Task<Room> Get(Guid id)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Rooms
                .Include(room => room.Equipment)
                .FirstOrDefaultAsync(r => r.IsActive && r.ID == id);
        }

        public async Task<ICollection<Room>> GetAllByFloor(int floor)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Rooms
                .Where(r => r.Floor == floor)
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<Room> GetByFloorAndNumber(int floor, string number)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Rooms
                .Where(r => r.Number == number)
                .Where(r => r.Floor == floor)
                .Where(r => r.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<Room>> GetAllByEquipmentType(EquipmentType equipmentType)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Rooms
                .Where(r => r.IsActive)
                .Include(r => r.Equipment
                    .Select(e => e.EquipmentType.Name == equipmentType.Name && e.IsActive))
                .ToListAsync();
        }

        public async Task<ICollection<Room>> GetAllByEquipmentTypes(ICollection<EquipmentType> equipmentTypes)
        {
            await using var context = ContextFactory.CreateDbContext();
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

        public async Task<ICollection<Room>> GetAllOccupied(DateTime start, DateTime end)
        {
            await using var context = ContextFactory.CreateDbContext();
            var occupiedRooms = await context.CalendarEntries
                .Include(ce => ce.Room)
                .Where(ce => ce.StartDateTime >= start && ce.EndDateTime <= end)
                .Select(ce => ce.Room)
                .Where(r => r.IsActive)
                .ToListAsync();

            return occupiedRooms.GroupBy(r => r.ID)
                .Select(g => g.First())
                .ToList();
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

        public async Task<(Room room, DateTime? timeSlotStart)> GetFirstFreeByTimeSlotAndDoctor(TimeSpan start, TimeSpan end, DateTime latestDate, Doctor doctor)
        {
            var startDate = DateTime.Today + start;
            var endDate = DateTime.Today + end;


            if ((await GetAllFree(startDate, endDate)).FirstOrDefault() == null)
            {
                return (null, null);
            }

            while (endDate.Date < latestDate.Date + TimeSpan.FromDays(1))
            {
                // Room and doctor are found in a given time frame and the first empty slot for the room needs to be found
                while (((await GetAllFree(startDate, startDate.AddMinutes(30))).FirstOrDefault() == null ||
                        await _doctorService.IsDoctorFreeInTimeSpan(startDate, startDate.AddMinutes(30), doctor) == false) &&
                       startDate < endDate)
                {
                    startDate = startDate.AddMinutes(30);
                }
                // See where the loop stopped (did the loop find a value or just loop to the end?)
                var doctorIsFree = await _doctorService.IsDoctorFreeInTimeSpan(startDate, startDate.AddMinutes(30), doctor);
                var freeRoom = (await GetAllFree(startDate, startDate.AddMinutes(30))).FirstOrDefault();

                if (doctorIsFree && freeRoom != null)
                {
                    return (freeRoom, startDate);
                }

                startDate = startDate.Date + TimeSpan.FromDays(1) + start;
                endDate = endDate.Date + TimeSpan.FromDays(1) + end;
            }

            return (null, null);
        }

        public async Task<Room> Create(int floor, string number, RoomType type)
        {
            var existingRoom = await GetByFloorAndNumber(floor, number);

            if (existingRoom != null)
            {
                throw new RoomAlreadyExistsException(floor, number);
            }

            var created = new Room
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
            room.Equipment?.Clear();
            room.IsActive = false;

            await Update(room);

            return true;
        }

        public async Task<ICollection<Room>> GetAllByRoomType(RoomType roomType)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Rooms
                .Where(r => r.IsActive)
                .Where(r => r.Type == roomType)
                .ToListAsync();
        }

        public async Task<Room> AddItems(Room room, ICollection<EquipmentItem> equipment)
        {
            room.Equipment = equipment;
            await Update(room);
            return room;
        }

        public async Task<Room> Update(Room room, int floor, string number, RoomType roomType, ICollection<EquipmentItem> equipment)
        {
            room.Floor = floor;
            room.Number = number;
            room.Type = roomType;
            room.Equipment = equipment;
            return await Update(room);
        }
    }
}