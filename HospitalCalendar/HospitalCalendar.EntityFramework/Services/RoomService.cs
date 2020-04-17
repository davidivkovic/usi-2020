using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.EntityFramework;
using HospitalCalendar.EntityFramework.Services;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services
{
    public class RoomService : GenericDataService<Room> , IRoomService
    {
        private readonly IEquipmentItemService _equipmentItemService;

        public RoomService(HospitalCalendarDbContextFactory contextFactory, IEquipmentItemService equipmentItemService) : base(contextFactory)
        {
            _equipmentItemService = equipmentItemService;
        }


        public async Task<ICollection<Room>> GetAllByFloor(int floor)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.Rooms
                    .Where(r => r.Floor == floor)
                    .ToListAsync();
            }
        }


        public async Task<Room> GetByFloorAndNumber(int floor, string number)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.Rooms
                    .Where(r => r.Number == number)
                    .Where(r => r.Floor == floor)
                    .FirstOrDefaultAsync();
            }
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


        public async Task<Room> Create(int floor, string number, RoomType type)
        {
            Room created = new Room()
            {
                Equipment = new List<EquipmentItem>(),
                Floor = floor,
                Number = number,
                IsActive = true,
                Type = type
            };

            return await base.Create(created);
        }

        public new async Task<bool> Delete(Guid id)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                // Cascade delete?
                var result = await base.Delete(id);

                _ = await _equipmentItemService.RefreshItems();

                return result;
            }
        }

        public async Task<ICollection<Room>> GetAllOccupied(DateTime start, DateTime end)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.CalendarEntries
                                    .Where(ce => ce.StartDateTime >= start && ce.EndDateTime <= end)
                                    .Select(x => x.Room)
                                    .ToListAsync();
            }

        }

        public async Task<ICollection<Room>> GetAllFree(DateTime start, DateTime end)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.CalendarEntries
                                    .Where(ce => ce.StartDateTime < start && ce.EndDateTime > end)
                                    .Select(x => x.Room)
                                    .ToListAsync();
            }
        }
    }
}