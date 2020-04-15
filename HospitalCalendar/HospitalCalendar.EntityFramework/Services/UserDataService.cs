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
    public class UserDataService : GenericDataService<User>, IUserService
    {

        public UserDataService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {

        }

        public async Task<User> GetByUsername(string username)
        {
            using (HospitalCalendarDbContext context = base._contextFactory.CreateDbContext())
            {
                return await context.Users
                                    .Where(u => u.IsActive == true)
                                    .FirstOrDefaultAsync(u => u.Username == username);
            }
        }

    }
}
