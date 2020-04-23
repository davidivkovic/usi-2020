using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HospitalCalendar.EntityFramework
{
    public class HospitalCalendarDbContextFactory : IDesignTimeDbContextFactory<HospitalCalendarDbContext>
    {
        public HospitalCalendarDbContext CreateDbContext(string[] args = null)
        {
            var options = new DbContextOptionsBuilder<HospitalCalendarDbContext>();
            options.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=HospitalCalendarDb;Trusted_connection=True");

            return new HospitalCalendarDbContext(options.Options);
        }
    }
}
