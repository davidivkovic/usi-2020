using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HospitalCalendar.EntityFramework.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EquipmentTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TotalAmount = table.Column<int>(nullable: false),
                    FreeAmount = table.Column<int>(nullable: false),
                    InUseAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    WorkingHoursStart = table.Column<TimeSpan>(nullable: true),
                    WorkingHoursEnd = table.Column<TimeSpan>(nullable: true),
                    Sex = table.Column<int>(nullable: true),
                    InsuranceNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "EquipmentItems",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    RoomID = table.Column<Guid>(nullable: true),
                    EquipmentTypeID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquipmentItems", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_EquipmentTypes_EquipmentTypeID",
                        column: x => x.EquipmentTypeID,
                        principalTable: "EquipmentTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquipmentItems_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Anamneses",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    PatientID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Anamneses", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Anamneses_Users_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentRequests",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    PatientID = table.Column<Guid>(nullable: true),
                    RequesterID = table.Column<Guid>(nullable: true),
                    ProposedDoctorID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AppointmentRequests_Users_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentRequests_Users_ProposedDoctorID",
                        column: x => x.ProposedDoctorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentRequests_Users_RequesterID",
                        column: x => x.RequesterID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorsPatients",
                columns: table => new
                {
                    DoctorId = table.Column<Guid>(nullable: false),
                    PatientId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorsPatients", x => new { x.DoctorId, x.PatientId });
                    table.ForeignKey(
                        name: "FK_DoctorsPatients_Users_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Users",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_DoctorsPatients_Users_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Users",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Specializations",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    SingleSpecialization = table.Column<int>(nullable: false),
                    DoctorID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specializations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Specializations_Users_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    DoctorID = table.Column<Guid>(nullable: true),
                    AnamnesisID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Entries_Anamneses_AnamnesisID",
                        column: x => x.AnamnesisID,
                        principalTable: "Anamneses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Entries_Users_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentRequestNotifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    AppointmentRequestID = table.Column<Guid>(nullable: true),
                    SecretaryID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentRequestNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AppointmentRequestNotifications_AppointmentRequests_AppointmentRequestID",
                        column: x => x.AppointmentRequestID,
                        principalTable: "AppointmentRequests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentRequestNotifications_Users_SecretaryID",
                        column: x => x.SecretaryID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEntries",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    StartDateTime = table.Column<DateTime>(nullable: false),
                    EndDateTime = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: true),
                    TypeID = table.Column<Guid>(nullable: true),
                    PatientID = table.Column<Guid>(nullable: true),
                    DoctorID = table.Column<Guid>(nullable: true),
                    RoomID = table.Column<Guid>(nullable: true),
                    IsUrgent = table.Column<bool>(nullable: true),
                    ChangingRoomType = table.Column<bool>(nullable: true),
                    MovingEquipment = table.Column<bool>(nullable: true),
                    ChangingLayout = table.Column<bool>(nullable: true),
                    NewRoomType = table.Column<int>(nullable: true),
                    Renovation_RoomID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEntries", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Users_DoctorID",
                        column: x => x.DoctorID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Users_PatientID",
                        column: x => x.PatientID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Specializations_TypeID",
                        column: x => x.TypeID,
                        principalTable: "Specializations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Rooms_Renovation_RoomID",
                        column: x => x.Renovation_RoomID,
                        principalTable: "Rooms",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentChangeRequests",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    PreviousStartDateTime = table.Column<DateTime>(nullable: false),
                    PreviousEndDateTime = table.Column<DateTime>(nullable: false),
                    NewStartDateTime = table.Column<DateTime>(nullable: false),
                    NewEndDateTime = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    AppointmentID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentChangeRequests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AppointmentChangeRequests_CalendarEntries_AppointmentID",
                        column: x => x.AppointmentID,
                        principalTable: "CalendarEntries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurgeryNotifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    SurgeryID = table.Column<Guid>(nullable: true),
                    SecretaryID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurgeryNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SurgeryNotifications_Users_SecretaryID",
                        column: x => x.SecretaryID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SurgeryNotifications_CalendarEntries_SurgeryID",
                        column: x => x.SurgeryID,
                        principalTable: "CalendarEntries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppointmentChangeRequestNotifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    AppointmentChangeRequestID = table.Column<Guid>(nullable: true),
                    SecretaryID = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentChangeRequestNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_AppointmentChangeRequestNotifications_AppointmentChangeRequests_AppointmentChangeRequestID",
                        column: x => x.AppointmentChangeRequestID,
                        principalTable: "AppointmentChangeRequests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppointmentChangeRequestNotifications_Users_SecretaryID",
                        column: x => x.SecretaryID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Anamneses_PatientID",
                table: "Anamneses",
                column: "PatientID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentChangeRequestNotifications_AppointmentChangeRequestID",
                table: "AppointmentChangeRequestNotifications",
                column: "AppointmentChangeRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentChangeRequestNotifications_SecretaryID",
                table: "AppointmentChangeRequestNotifications",
                column: "SecretaryID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentChangeRequests_AppointmentID",
                table: "AppointmentChangeRequests",
                column: "AppointmentID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequestNotifications_AppointmentRequestID",
                table: "AppointmentRequestNotifications",
                column: "AppointmentRequestID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequestNotifications_SecretaryID",
                table: "AppointmentRequestNotifications",
                column: "SecretaryID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequests_PatientID",
                table: "AppointmentRequests",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequests_ProposedDoctorID",
                table: "AppointmentRequests",
                column: "ProposedDoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_AppointmentRequests_RequesterID",
                table: "AppointmentRequests",
                column: "RequesterID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_DoctorID",
                table: "CalendarEntries",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_PatientID",
                table: "CalendarEntries",
                column: "PatientID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_RoomID",
                table: "CalendarEntries",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_TypeID",
                table: "CalendarEntries",
                column: "TypeID");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_Renovation_RoomID",
                table: "CalendarEntries",
                column: "Renovation_RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorsPatients_PatientId",
                table: "DoctorsPatients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_AnamnesisID",
                table: "Entries",
                column: "AnamnesisID");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_DoctorID",
                table: "Entries",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_EquipmentTypeID",
                table: "EquipmentItems",
                column: "EquipmentTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_EquipmentItems_RoomID",
                table: "EquipmentItems",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Specializations_DoctorID",
                table: "Specializations",
                column: "DoctorID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryNotifications_SecretaryID",
                table: "SurgeryNotifications",
                column: "SecretaryID");

            migrationBuilder.CreateIndex(
                name: "IX_SurgeryNotifications_SurgeryID",
                table: "SurgeryNotifications",
                column: "SurgeryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppointmentChangeRequestNotifications");

            migrationBuilder.DropTable(
                name: "AppointmentRequestNotifications");

            migrationBuilder.DropTable(
                name: "DoctorsPatients");

            migrationBuilder.DropTable(
                name: "Entries");

            migrationBuilder.DropTable(
                name: "EquipmentItems");

            migrationBuilder.DropTable(
                name: "SurgeryNotifications");

            migrationBuilder.DropTable(
                name: "AppointmentChangeRequests");

            migrationBuilder.DropTable(
                name: "AppointmentRequests");

            migrationBuilder.DropTable(
                name: "Anamneses");

            migrationBuilder.DropTable(
                name: "EquipmentTypes");

            migrationBuilder.DropTable(
                name: "CalendarEntries");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Specializations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
