using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services.EquipmentServices
{
    public class EquipmentTypeService : GenericDataService<EquipmentType>, IEquipmentTypeService
    {
        public EquipmentTypeService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }


        public async Task<EquipmentType> Create(string name, string description, int amount)
        {
            EquipmentType created = new EquipmentType()
            {
                Name = name,
                Description = description,
                InUseAmount = 0,
                TotalAmount = amount,
                FreeAmount = amount
            };

            return await Create(created);
        }


        public async Task<EquipmentType> Update(EquipmentType entity, string name, string description, int amount)
        {
            if (amount < entity.InUseAmount)
            {
                // TODO: Throw exception when reducing item count below allowed
                // cant reduce number of items beyond than what is in use
            }

            entity.Name = name;
            entity.Description = description;
            entity.TotalAmount = amount;
            entity.FreeAmount = entity.TotalAmount - entity.InUseAmount;

            return await Update(entity);
        }

        public new async Task<bool> Delete(Guid id)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                await context.EquipmentTypes.ForEachAsync(async et =>
                    {
                        var discardedItems = context
                            .EquipmentItems
                            .Where(ei => ei.EquipmentType.Name == et.Name);

                        await discardedItems.ForEachAsync(async di =>
                        {
                            await Delete(di.ID);
                        });
                    });

                return true;
            }
        }

        public async Task<bool> EnsureCapacity()
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                await context.EquipmentTypes.ForEachAsync(async et =>
                {
                    var inUse = context
                        .EquipmentItems
                        .Where(ei => ei.EquipmentType.Name == et.Name)
                        .Count(ei => ei.Room != null);

                    et.InUseAmount = inUse;
                    et.FreeAmount = et.TotalAmount - et.InUseAmount;

                    _ = await Update(et);
                });

                return true;
            }
        }

        public async Task<EquipmentType> GetByName(string name)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.EquipmentTypes
                                    .Where(e => e.Name == name)
                                    .FirstOrDefaultAsync();
            }
        }
    }
}
