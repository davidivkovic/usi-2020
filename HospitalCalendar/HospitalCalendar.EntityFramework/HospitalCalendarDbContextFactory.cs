using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HospitalCalendar.EntityFramework
{
    public class HospitalCalendarDbContextFactory : IDesignTimeDbContextFactory<HospitalCalendarDbContext>
    {
        public HospitalCalendarDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<HospitalCalendarDbContext>();
            options.UseSqlServer("Server=localhost;Database=HospitalCalendarDb;User ID=SA;Password=time4Popcorn;Trusted_connection=False");

            return new HospitalCalendarDbContext(options.Options);
        }
    }
}
