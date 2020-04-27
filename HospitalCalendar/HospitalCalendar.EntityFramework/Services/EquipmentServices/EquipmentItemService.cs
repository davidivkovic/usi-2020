using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalCalendar.EntityFramework.Services.EquipmentServices
{
    public class EquipmentItemService : GenericDataService<EquipmentItem>, IEquipmentItemService
    {
        public EquipmentItemService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory) { }

        public async Task<ICollection<EquipmentItem>> GetAllByType(EquipmentType equipmentType)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.EquipmentItems
                    .Where(e => e.EquipmentType.Name == equipmentType.Name)
                    .ToListAsync();
            }
        }

        public async Task<bool> Create(EquipmentType equipmentType, int count)
        {
 

            return await Task.Run(() =>
            {
                Enumerable.Range(1, count).ToList().ForEach(async i =>
                {
                    var equipmentItem = new EquipmentItem()
                    {
                        Room = null,
                        EquipmentType = equipmentType,
                        IsActive = true
                    };

                    _ = await Create(equipmentItem);
                });

                return true;
            });
        }

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
