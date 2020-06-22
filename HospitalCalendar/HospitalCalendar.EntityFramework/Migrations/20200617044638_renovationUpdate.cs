using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalCalendar.EntityFramework.Migrations
{
    public partial class renovationUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "CalendarEntries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "CalendarEntries");
        }
    }
}
