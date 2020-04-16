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

        public async Task<bool> EnsureCapacity()
        {
            
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
