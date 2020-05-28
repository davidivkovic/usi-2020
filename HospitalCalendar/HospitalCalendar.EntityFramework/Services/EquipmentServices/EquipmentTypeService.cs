using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.EntityFramework.Exceptions;
using System.Collections.Generic;

namespace HospitalCalendar.EntityFramework.Services.EquipmentServices
{
    public class EquipmentTypeService : GenericDataService<EquipmentType>, IEquipmentTypeService
    {
        private readonly IEquipmentItemService _equipmentItemService;

        public EquipmentTypeService(HospitalCalendarDbContextFactory contextFactory, IEquipmentItemService equipmentItemService) : base(contextFactory)
        {
            _equipmentItemService = equipmentItemService;
        }



        public async Task<EquipmentType> GetByName(string name)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();

            return await context.EquipmentTypes
                .Where(e => e.Name == name)
                .Where(e => e.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<EquipmentType> Create(string name, string description, int amount)
        {
            var existingEquipmentType = await GetByName(name);

            if (existingEquipmentType != null)
            {
                throw new EquipmentTypeAlreadyExistsException(name);
            }

            EquipmentType equipmentType = new EquipmentType()
            {
                Name = name,
                Description = description,
                IsActive = true
            };

            var createdEquipmentType = await Create(equipmentType);
            // Fire and forget
            await Task.Factory.StartNew(() => _equipmentItemService.Create(createdEquipmentType, amount));

            return createdEquipmentType;
        }

        public async Task<EquipmentType> Update(EquipmentType equipmentType, string name, string description, int amountDelta)
        {
            equipmentType.Name = name;
            equipmentType.Description = description;

            if (amountDelta < 0)
            {
                await Task.Factory.StartNew(() => _equipmentItemService.Remove(equipmentType, amountDelta));
            }
            else
            {
                await Task.Factory.StartNew(() => _equipmentItemService.Create(equipmentType, amountDelta));
            }

            return await Update(equipmentType);
        }

        public async Task<bool> PhysicalDelete(Guid id)
        {
            return await base.Delete(id);
        }

        public new async Task<bool> Delete(Guid id)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();
            var equipmentType = await Get(id);

            equipmentType.IsActive = false;

            await context.EquipmentItems
                .Include(ei => ei.EquipmentType)
                .Where(ei => ei.EquipmentType.Name == equipmentType.Name)
                .ForEachAsync(async ei =>
                {
                    ei.IsActive = false;
                    await _equipmentItemService.Update(ei);
                });

            await Update(equipmentType);

            return true;
        }

        public async Task<List<EquipmentType>> GetAllByRoom(Room room)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();
            return context.Rooms
                    .Include(r => r.Equipment)
                    .ThenInclude(e => e.EquipmentType)
                    .First(r => r.IsActive && room.ID == r.ID)
                    .Equipment
                    .Select(e => e.EquipmentType)
                    .GroupBy(et => et.Name)
                    .Select(g => g.First())
                    .ToList();
        }
    }
}