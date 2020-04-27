using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class AppointmentService : GenericDataService<Appointment>,IAppointmentService
    {
        public AppointmentService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
            

        }

        public async Task<ICollection<Appointment>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.StartDateTime >= start && a.EndDateTime <= end)
                                    .ToListAsync(); 
            }

        }


        public async Task<ICollection<Appointment>> GetAllByStatus(AppointmentStatus status) 
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.Status==status)
                                    .ToListAsync();
            }

        }


        public async Task<ICollection<Appointment>> GetAllDoctor(Doctor doctor) 
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext()) 
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.Doctor.ID == doctor.ID)
                                    .ToListAsync();
            }
        
        }

        public async Task<ICollection<Appointment>> GetAllPatient(Patient patient)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Appointments
                                    .Where(a => a.IsActive)
                                    .Where(a => a.Patient.ID == patient.ID)
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
                Patient=patient,
                Doctor=doctor,
                Type = type
            };
            
            _ = await Create(appointment);

            return appointment;
        }

        public async Task<Appointment> Update(Appointment entity,DateTime start, DateTime end, Patient patient, Doctor doctor, Specialization type, AppointmentStatus status) 
        {
            entity.StartDateTime = start;
            entity.EndDateTime = end;
            entity.Patient = patient;
            entity.Doctor = doctor;
            entity.Type = type;
            entity.Status = status;

            _ = await Update(entity);

            return entity;
        }
    }
}
