using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Immutable;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class AppointmentService : GenericDataService<Appointment>, IAppointmentService
    {
        public AppointmentService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }

        public async Task<ICollection<Appointment>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Include(a => a.Room)
                                    .Where(a => a.IsActive)
                                    .Where(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                                                         (a.EndDateTime >= start && a.EndDateTime <= end) ||
                                                         (a.StartDateTime >= start && a.EndDateTime <= end))
                                    .ToListAsync();
            }
        }

        public async Task<ICollection<Appointment>> GetAllByStatus(AppointmentStatus status)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.Status == status)
                                    .ToListAsync();
            }
        }

        public async Task<ICollection<Appointment>> GetAllByDoctor(Doctor doctor)
        {
            await using HospitalCalendarDbContext context = ContextFactory.CreateDbContext();
            return await context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .Include(a => a.Type)
                .Include(a => a.Room)
                .Where(a => a.IsActive)
                .Where(a => a.Doctor.ID == doctor.ID)
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAllByPatient(Patient patient)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.Patient.ID == patient.ID)
                                    .ToListAsync();
            }
        }

        public async Task<ICollection<Appointment>> GetAllByRoom(Room room)
        {
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.Room.ID == room.ID)
                                    .ToListAsync();
            }
        }

        public async Task<Appointment> Create(DateTime start, DateTime end, Patient patient, Doctor doctor, Specialization type)
        {
            Appointment appointment = new Appointment()
            {
                StartDateTime = start,
                EndDateTime = end,
                Status = AppointmentStatus.Scheduled,
                Patient = patient,
                Doctor = doctor,
                Type = type
            };

            await Create(appointment);

            return appointment;
        }

        public async Task<Appointment> Update(Appointment appointment, DateTime start, DateTime end, Patient patient, Doctor doctor, Specialization type, AppointmentStatus status)
        {
            appointment.StartDateTime = start;
            appointment.EndDateTime = end;
            appointment.Patient = patient;
            appointment.Doctor = doctor;
            appointment.Type = type;
            appointment.Status = status;

            await Update(appointment);

            return appointment;
        }
    }
}