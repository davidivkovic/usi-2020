using HospitalCalendar.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.NotificationsServices;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class SurgeryService : GenericDataService<Surgery>, ISurgeryService
    {
        private readonly INotificationService _notificationService;
        public SurgeryService(HospitalCalendarDbContextFactory contextFactory, INotificationService notificationService) : base(contextFactory)
        {
            _notificationService = notificationService;
        }

        public async Task<ICollection<Surgery>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Surgeries
                                .Include(a => a.Room)
                                .Where(a => a.IsActive)
                                .Where(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                                            (a.EndDateTime >= start && a.EndDateTime <= end) ||
                                            (a.StartDateTime >= start && a.EndDateTime <= end))
                                .ToListAsync();
        }

        public async Task<ICollection<Surgery>> GetAllByDoctor(Doctor doctor)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Surgeries
                                .Include(s => s.Type)
                                .Include(s => s.Patient)
                                .Include(s => s.Doctor)
                                .Include(s => s.Room)
                                .Where(a => a.IsActive)
                                .Where(a => a.Doctor.ID == doctor.ID)
                                .ToListAsync();
        }

        public async Task<ICollection<Surgery>> GetAllByPatient(Patient patient)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Surgeries
                .Include(s => s.Type)
                .Include(s => s.Patient)
                .Include(s => s.Doctor)
                .Include(s => s.Room)
                .Where(a => a.IsActive)
                .Where(a => a.Patient.ID == patient.ID)
                .ToListAsync();
        }

        public async Task<AppointmentRequest> CreateSurgeryRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor, bool isUrgent, DateTime timestamp, Room room)
        {
            var surgeryRequest = new SurgeryRequest
            {
                IsActive = true,
                Room = room,
                StartDate = start,
                EndDate = end,
                IsApproved = false,
                Patient = patient,
                Requester = requester,
                ProposedDoctor = proposedDoctor,
                IsUrgent = isUrgent
            };

            await using var context = ContextFactory.CreateDbContext();
            var createdSurgeryRequest = (await context.SurgeryRequests.AddAsync(surgeryRequest)).Entity;

            await _notificationService.PublishSurgeryRequestNotification(createdSurgeryRequest, timestamp, string.Empty);

            return createdSurgeryRequest;
        }

        public async Task<Surgery> Create(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room, bool isUrgent)
        {
            var surgery = new Surgery();
            await Create(surgery);
            surgery.StartDateTime = start;
            surgery.EndDateTime = end;
            surgery.Doctor = doctor;
            surgery.Patient = patient;
            surgery.Room = room;
            surgery.IsUrgent = isUrgent;
            surgery.Type = doctor.Specializations.FirstOrDefault();
            surgery.IsActive = true;
            surgery.Status = AppointmentStatus.Scheduled;

            // This contains a unique ID
            surgery = await Update(surgery);
            // DateTime.Now I cant unit test this
            if(isUrgent)
                await _notificationService.PublishSurgeryNotification(surgery, DateTime.Now, string.Empty);
            return surgery;
        }
    }
}