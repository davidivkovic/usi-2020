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

                // TODO: If equipment count doesnt add up change the next lines to match the previous

                // Added in renovations
                var addedEquipmentItems = context.Renovations
                    .Include(r => r.AddedEquipmentItems)
                    .ThenInclude(ei => ei.EquipmentType)
                    .SelectMany(x => x.AddedEquipmentItems)
                    .Where(aei => aei.EquipmentType.Name == equipmentType.Name)
                    .ToList();

                // Removed in renovations
                var removedEquipmentItems = context.Renovations
                    .Include(r => r.RemovedEquipmentItems)
                    .ThenInclude(ei => ei.EquipmentType)
                    .SelectMany(x => x.RemovedEquipmentItems)
                    .Where(rei => rei.EquipmentType.Name == equipmentType.Name)
                    .ToList();

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
                    .Select(i => new EquipmentItem {IsActive = true})
                    .ToList();

                context.EquipmentItems.AddRange(createdEquipmentItems);
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
            await Task.Factory.StartNew(async () =>
            {
                context.EquipmentItems.RemoveRange(freeEquipmentItems);
                await context.SaveChangesAsync();
            });
            return true;
        }

        public async Task<ICollection<EquipmentItem>> GetAllWithoutRoom()
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.EquipmentItems
                    .Where(ei => ei.IsActive)
                    .Where(ei => ei.Room == null)
                    .ToListAsync();
            }
        }

        public async Task<ICollection<EquipmentItem>> GetAllByRoom(Room room)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.EquipmentItems
                    .Where(ei => ei.IsActive)
                    .Where(ei => ei.Room.ID == room.ID)
                    .ToListAsync();
            }
        }

        public async Task<ICollection<EquipmentItem>> GetAllByTypeInRoom(EquipmentType equipmentType, Room room)
        {
            await using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return context.Rooms
                    .Where(r => r.IsActive)
                    .Include(r => r.Equipment)
                    .ThenInclude(e => e.EquipmentType)
                    .First(r => r.ID == room.ID)
                    .Equipment
                    .Where(ei => ei.EquipmentType.Name == equipmentType.Name)
                    .ToList();
            }
        }
    }
}