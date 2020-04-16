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
    public class RoomService : GenericDataService<Room> , IRoomService
    {
        public RoomService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {

        }
        public async Task<ICollection<Room>> GetAllByEquipmentType(EquipmentType equipmentType)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.Rooms
                                    .Include(r => r.Equipment
                                    .Select(e => e.EquipmentType.Name == equipmentType.Name))
                                    .ToListAsync();
            }
        }   


        public async Task<ICollection<Room>> GetAllByEquipmentTypes(ICollection<EquipmentType> equipmentTypes) 
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext()) 
            {
                return await context.Rooms
                                    .Include(r => r.Equipment
                                    .Where(e =>  equipmentTypes
                                    .All(et => (et.Name== e.EquipmentType.Name))))
                                    .ToListAsync();
            }
        }
    }
}