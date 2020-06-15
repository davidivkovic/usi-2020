using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalCalendar.EntityFramework.Services.EquipmentServices
{
    public class EquipmentItemService : GenericDataService<EquipmentItem>, IEquipmentItemService
    {
        public EquipmentItemService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<ICollection<EquipmentItem>> GetAllByType(EquipmentType equipmentType)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.EquipmentItems
                .Where(ei => ei.IsActive)
                .Where(ei => ei.EquipmentType.Name == equipmentType.Name)
                .Include(ei => ei.EquipmentType)
                .ToListAsync();
        }

        public async Task<ICollection<EquipmentItem>> GetAllInUseByType(EquipmentType equipmentType)
        {
            return await Task.Run(async () =>
            {
                await using var context = ContextFactory.CreateDbContext();

                var equipmentItemsInRooms = await context.Rooms
                    .Include(r => r.Equipment)
                    .ThenInclude(ei => ei.EquipmentType)
                    .Where(eir => eir.Equipment.Any(eee => eee.EquipmentType.Name == equipmentType.Name))
                    .SelectMany(r => r.Equipment)
                    .ToListAsync();

                // TODO: If equipment count doesn't add up change the next lines to match the previous

                // Added in renovations
                var addedEquipmentItems = await context.Renovations
                    .Include(r => r.AddedEquipmentItems)
                    .ThenInclude(ei => ei.EquipmentType)
                    .SelectMany(x => x.AddedEquipmentItems)
                    .Where(aei => aei.EquipmentType.Name == equipmentType.Name)
                    .ToListAsync();

                // Removed in renovations
                var removedEquipmentItems = await context.Renovations
                    .Include(r => r.RemovedEquipmentItems)
                    .ThenInclude(ei => ei.EquipmentType)
                    .SelectMany(x => x.RemovedEquipmentItems)
                    .Where(rei => rei.EquipmentType.Name == equipmentType.Name)
                    .ToListAsync();

                //var emptyList = Enumerable.Empty<EquipmentItem>();

                var allItemsInUse = equipmentItemsInRooms;
                allItemsInUse = allItemsInUse.Union(addedEquipmentItems).ToList();
                allItemsInUse = allItemsInUse.Union(removedEquipmentItems).ToList();

                var allItemsInUseDistinct = allItemsInUse
                    .GroupBy(ei => ei.ID)
                    .Select(g => g.First())
                    .ToList();

                return allItemsInUseDistinct;
            });
        }

        public async Task<ICollection<EquipmentItem>> GetAllFreeByType(EquipmentType equipmentType)
        {
            var allEquipmentItems = await GetAllByType(equipmentType);
            var equipmentItemsInUse = await GetAllInUseByType(equipmentType);

            return await Task.Run(() =>
            {
                var freeEquipmentItems = allEquipmentItems
                    .Where(ei => equipmentItemsInUse
                        .All(eiu => eiu.ID != ei.ID))
                    .ToList();
                return freeEquipmentItems;
            });
        }

        public async Task<bool> Create(EquipmentType equipmentType, int amount)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await Task.Run(async () =>
            {
                var createdEquipmentItems = Enumerable
                    .Range(1, amount)
                    .Select(i => new EquipmentItem { IsActive = true })
                    .ToList();

                await context.EquipmentItems.AddRangeAsync(createdEquipmentItems);
                await context.SaveChangesAsync();

                createdEquipmentItems.ForEach(ei => ei.EquipmentType = equipmentType);
                context.EquipmentItems.UpdateRange(createdEquipmentItems);
                await context.SaveChangesAsync();

                return true;
            });
        }

        public async Task<bool> Remove(EquipmentType equipmentType, int amount)
        {
            await using var context = ContextFactory.CreateDbContext();
            var freeEquipmentItems = await GetAllFreeByType(equipmentType);
            freeEquipmentItems = freeEquipmentItems.Take(Math.Abs(amount)).ToList();
            await Task.Run(async () =>
            {
                context.EquipmentItems.RemoveRange(freeEquipmentItems);
                await context.SaveChangesAsync();
            });
            return true;
        }

        public async Task<ICollection<EquipmentItem>> GetAllWithoutRoom()
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.EquipmentItems
                .Where(ei => ei.IsActive)
                .Where(ei => ei.Room == null)
                .ToListAsync();
        }

        public async Task<ICollection<EquipmentItem>> GetAllByRoom(Room room)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.EquipmentItems
                .Where(ei => ei.IsActive)
                .Where(ei => ei.Room.ID == room.ID)
                .ToListAsync();
        }

        public async Task<ICollection<EquipmentItem>> GetAllByTypeInRoom(EquipmentType equipmentType, Room room)
        {
            await using var context = ContextFactory.CreateDbContext();
            return context.Rooms
                .Where(r => r.IsActive)
                .Include(r => r.Equipment)
                .ThenInclude(e => e.EquipmentType)
                .First(r => r.ID == room.ID)
                .Equipment
                .Where(ei => ei.EquipmentType.Name == equipmentType.Name)
                .ToList();
        }

        public async Task<int> CountByType(EquipmentType equipmentType)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.EquipmentItems
                .Include(ei => ei.EquipmentType)
                .CountAsync(ei => ei.EquipmentType.Name == equipmentType.Name);
        }

        public async Task<int> CountFreeByType(EquipmentType equipmentType)
        {
            return await CountByType(equipmentType) - await CountInUseByType(equipmentType);
        }

        public async Task<int> CountInUseByType(EquipmentType equipmentType)
        {
            await using var context = ContextFactory.CreateDbContext();
            var result = 0;

            // Items in rooms
            result += await context.Rooms.Include(r => r.Equipment)
                .ThenInclude(ei => ei.EquipmentType)
                .SelectMany(r => r.Equipment)
                .CountAsync(ei => ei.EquipmentType.Name == equipmentType.Name);

            // Added in renovations
            result += await context.Renovations
                .Include(r => r.AddedEquipmentItems)
                .ThenInclude(ei => ei.EquipmentType)
                .SelectMany(x => x.AddedEquipmentItems)
                .CountAsync(aei => aei.EquipmentType.Name == equipmentType.Name);

            // Removed in renovations
            result += await context.Renovations
                .Include(r => r.RemovedEquipmentItems)
                .ThenInclude(ei => ei.EquipmentType)
                .SelectMany(x => x.RemovedEquipmentItems)
                .CountAsync(rei => rei.EquipmentType.Name == equipmentType.Name);

            return result;
        }

        public async Task<int> CountByTypeInRoom(EquipmentType equipmentType, Room room)
        {
            await using var context = ContextFactory.CreateDbContext();
            // Items in rooms
            return await context.Rooms.Include(r => r.Equipment)
                .ThenInclude(ei => ei.EquipmentType)
                .Where(r => r.ID == room.ID)
                .SelectMany(r => r.Equipment)
                .CountAsync(ei => ei.EquipmentType.Name == equipmentType.Name);
        }
    }
}