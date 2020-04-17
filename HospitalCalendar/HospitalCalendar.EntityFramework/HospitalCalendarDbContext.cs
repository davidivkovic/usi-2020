using HospitalCalendar.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework
{
    public class HospitalCalendarDbContext : DbContext
    {
        public DbSet<Anamnesis> Anamneses { get; set; }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<AppointmentChangeRequest> AppointmentChangeRequests { get; set; }

        public DbSet<AppointmentChangeRequestNotification> AppointmentChangeRequestNotifications { get; set; }

        public DbSet<AppointmentRequest> AppointmentRequests { get; set; }

        public DbSet<AppointmentRequestNotification> AppointmentRequestNotifications { get; set; }

        public DbSet<CalendarEntry> CalendarEntries { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<Entry> Entries { get; set; }

        public DbSet<EquipmentItem> EquipmentItems { get; set; }

        public DbSet<EquipmentType> EquipmentTypes { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Patient> Patients { get; set; }

        public DbSet<Renovation> Renovations { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Secretary> Secretaries { get; set; }

        public DbSet<Specialization> Specializations { get; set; }

        public DbSet<Surgery> Surgeries { get; set; }

        public DbSet<SurgeryNotification> SurgeryNotifications { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<DoctorPatient> DoctorsPatients { get; set; }

        public HospitalCalendarDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorPatient>().HasKey(dp => new { dp.DoctorId, dp.PatientId });

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Doctor)
                .WithMany(d => d.DoctorsPatients)
                .HasForeignKey(dp => dp.DoctorId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DoctorPatient>()
                .HasOne(dp => dp.Patient)
                .WithMany(p => p.DoctorsPatients)
                .HasForeignKey(dp => dp.PatientId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<EquipmentType>()
                .HasIndex(ei => ei.Name)
                .IsUnique();
        }
    }
}
