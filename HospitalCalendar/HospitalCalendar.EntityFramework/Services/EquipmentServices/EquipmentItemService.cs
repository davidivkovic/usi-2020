using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.EquipmentServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HospitalCalendar.EntityFramework.Services.EquipmentServices
{
    public class EquipmentItemService : GenericDataService<EquipmentItem>, IEquipmentItemService
    {
        private readonly IEquipmentTypeService _equipmentTypeService;

        public EquipmentItemService(HospitalCalendarDbContextFactory contextFactory, IEquipmentTypeService equipmentTypeService) : base(contextFactory)
        {
            _equipmentTypeService = equipmentTypeService;
        }


        public async Task<ICollection<EquipmentItem>> GetAllByType(EquipmentType equipmentType)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.EquipmentItems
                    .Where(e => e.EquipmentType.Name == equipmentType.Name)
                    .ToListAsync();
            }

        }

        public async Task<bool> RefreshItems()
        {
            return await _equipmentTypeService.EnsureCapacity();
        }
    }
}
