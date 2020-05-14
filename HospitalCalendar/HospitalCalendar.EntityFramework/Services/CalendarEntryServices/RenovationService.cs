using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class RenovationService : GenericDataService<Renovation>, IRenovationService
    {
        public RenovationService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            Renovation entity = new Renovation()
            {
                Room = room,
                RoomToAdd = roomToAdd,
                NewRoomType = newRoomType,
                StartDateTime = start,
                EndDateTime = end,
                Splitting = splitting,
                RemovedEquipmentItems = removedEquipmentItems,
                AddedEquipmentItems = addedEquipmentItems
            };

            _ = await Create(entity);

            return entity;
        }

        public async Task<Renovation> Update(Renovation entity,Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end, bool splitting,
            ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            entity.Room = room;
            entity.RoomToAdd = roomToAdd;
            entity.NewRoomType = newRoomType;
            entity.StartDateTime = start;
            entity.EndDateTime = end;
            entity.Splitting = splitting;
            entity.RemovedEquipmentItems = removedEquipmentItems;
            entity.AddedEquipmentItems = addedEquipmentItems;
           

            _ = await Update(entity);

            return entity;
        }

        public async Task<ICollection<Renovation>> GetAllByTimeFrame(DateTime start, DateTime end) 
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext()) 
            {
                return await context.Renovations
                                    .Where(r => r.IsActive)
                                    .Where(r => r.StartDateTime >= start && r.EndDateTime <= end)
                                    .ToListAsync();
            }
        }
    }
}
