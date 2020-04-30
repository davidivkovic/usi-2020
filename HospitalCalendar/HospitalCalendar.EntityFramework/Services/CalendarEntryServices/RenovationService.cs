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

        public Task<Renovation> Create(Room room, Room roomToAdd, RoomType newRoomType, DateTime start, DateTime end,
            bool splitting, ICollection<EquipmentItem> removedEquipmentItems, ICollection<EquipmentItem> addedEquipmentItems)
        {
            throw new NotImplementedException();
        }
        /*
public new async Task<Renovation> Create(List<EquipmentItem> addedEquipmentItems)
{
   using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
   {
       addedEquipmentItems.ForEach(aei =>
       {
           var addedEquipmentItemsToUpdate = context.EquipmentItems
               .FirstOrDefaultAsync(ei => ei.ID == aei.ID).Result;

           addedEquipmentItemsToUpdate.
       });


       var createdRenovation = new Renovation()
       {

       };
       return createdRenovation;
   }
}
*/
    }
}
