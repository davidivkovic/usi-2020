using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.EquipmentTypes
                                    .Where(e => e.Name == name)
                                    .Where(e => e.IsActive)
                                    .FirstOrDefaultAsync();
            }
        }

        public async Task<EquipmentType> Create(string name, string description)
        {
            EquipmentType equipmentType = new EquipmentType()
            {
                Name = name,
                Description = description
            };

            _ = await Create(equipmentType);

            return equipmentType;
        }

        public async Task<EquipmentType> Update(EquipmentType entity, string name, string description)
        {
            entity.Name = name;
            entity.Description = description;

            return await Update(entity);
        }


        public new async Task<bool> Delete(Guid id)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                var equipmentType = await Get(id);

                equipmentType.IsActive = false;

                await context.EquipmentItems
                    .Include(ei => ei.EquipmentType)
                    .Where(ei => ei.EquipmentType.Name == equipmentType.Name)
                    .ForEachAsync(async ei =>
                    {
                        ei.IsActive = false;
                        _ = await _equipmentItemService.Update(ei);
                    });

                _ = await Update(equipmentType);

                return true;
            }
        }
    }
}
