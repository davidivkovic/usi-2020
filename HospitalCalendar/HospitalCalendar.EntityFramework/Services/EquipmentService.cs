using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework;
using HospitalCalendar.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;


namespace HospitalCalendar.EntityFramework.Services
{
    public class EquipmentService : GenericDataService<EquipmentItem>, IEquipmentService
    {

        public EquipmentService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {

        }

        public async Task<List<EquipmentItem>> GetAllByName(string name) 
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext()) 
            {
                return await context.EquipmentItems
                                    .Where(e => e.EquipmentType.Name == name)
                                    .ToListAsync();
            }
            
        }


    }
}
