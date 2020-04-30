using System;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.EntityFramework.Services.CalendarEntryServices;

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
                var equipmentItemsInRooms = context.Rooms
                    .Include(r => r.Equipment)
                        .ThenInclude(ei => ei.EquipmentType)
                    .Select(r => r.Equipment)
                    .FirstOrDefault()?
                    .Where(ei => ei.EquipmentType.Name == equipmentType.Name)
                    .ToList();

                // Added in renovations
                var addedEquipmentItems = context.Renovations
                    .Include(r=> r.AddedEquipmentItems)
                        .ThenInclude(ei => ei.EquipmentType)
                    .Select(x=> x.AddedEquipmentItems)
                    .FirstOrDefault()?
                    .Where(aei => aei.EquipmentType.Name == equipmentType.Name)
                    .ToList();

                // Removed in renovations
                var removedEquipmentItems = context.Renovations
                    .Include(r => r.RemovedEquipmentItems)
                        .ThenInclude(ei => ei.EquipmentType)
                    .Select(x => x.RemovedEquipmentItems)
                    .FirstOrDefault()?
                    .Where(rei => rei.EquipmentType.Name == equipmentType.Name)
                    .ToList();

                var emptyList = Enumerable.Empty<EquipmentItem>();

                var allItemsInUse = equipmentItemsInRooms ?? emptyList;
                allItemsInUse = allItemsInUse?.Union(addedEquipmentItems ?? emptyList); 
                allItemsInUse = allItemsInUse?.Union(removedEquipmentItems ?? emptyList);

                var allItemsInUseDistinct = allItemsInUse?
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

            return await Task.Run(async() =>
            {
                var createdEquipmentItems = Enumerable
                    .Range(1, amount)
                    .Select(i => new EquipmentItem{IsActive =  true})
                    .ToList();

                context.EquipmentItems.AddRange(createdEquipmentItems);
                await context.SaveChangesAsync();

                createdEquipmentItems.ForEach(ei=>ei.EquipmentType = equipmentType);
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

            context.EquipmentItems.RemoveRange(freeEquipmentItems);
            await context.SaveChangesAsync();

            return true;

        public async Task<ICollection<EquipmentItem>> GetAllWithoutRoom() 
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.EquipmentItems
                                    .Where(ei => ei.IsActive)
                                    .Where(ei => ei.Room == null)
                                    .ToListAsync();
            }
        }
    }
}
