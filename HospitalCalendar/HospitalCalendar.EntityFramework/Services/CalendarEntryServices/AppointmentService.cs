using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Enums;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.NotificationsServices;
using HospitalCalendar.Domain.Services.UserServices;
using Microsoft.EntityFrameworkCore;

namespace HospitalCalendar.EntityFramework.Services.CalendarEntryServices
{
    public class AppointmentService : GenericDataService<Appointment>, IAppointmentService
    {
        private readonly INotificationService _notificationService;
        private readonly IRoomService _roomService;
        private readonly IDoctorService _doctorService;


        public AppointmentService(HospitalCalendarDbContextFactory contextFactory, INotificationService notificationService,
            IRoomService roomService, IDoctorService doctorService) : base(contextFactory)
        {
            _notificationService = notificationService;
            _roomService = roomService;
            _doctorService = doctorService;
        }

        public async Task<ICollection<Appointment>> GetAllByTimeFrame(DateTime start, DateTime end)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Appointments
                .Include(a => a.Room)
                .Include(a => a.Doctor)
                .Where(a => a.IsActive)
                .Where(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                            (a.EndDateTime >= start && a.EndDateTime <= end) ||
                            (a.StartDateTime >= start && a.EndDateTime <= end))
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAllByStatus(AppointmentStatus status)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Appointments
                .Where(a => a.IsActive)
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAllByDoctor(Doctor doctor)
        {
            await using var context = ContextFactory.CreateDbContext();
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
            await using var context = ContextFactory.CreateDbContext();
            return await context.Appointments
                .Include(a => a.Room)
                .Include(a => a.Patient)
                .Include(a => a.Type)
                .Where(a => a.IsActive)
                .Where(a => a.Patient.ID == patient.ID)
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAllByRoom(Room room)
        {
            await using var context = ContextFactory.CreateDbContext();
            return await context.Appointments
                .Where(a => a.IsActive)
                .Where(a => a.Room.ID == room.ID)
                .ToListAsync();
        }

        public async Task<Appointment> Create(DateTime start, DateTime end, Doctor doctor, Patient patient, Room room)
        {
            var appointment = new Appointment();
            appointment = await Create(appointment);
            appointment.IsActive = true;
            appointment.StartDateTime = start;
            appointment.EndDateTime = end;
            appointment.Doctor = doctor;
            appointment.Patient = patient;
            appointment.Room = room;
            appointment.Status = AppointmentStatus.Scheduled;
            appointment.Type = doctor.Specializations?.FirstOrDefault();

            return await Update(appointment);
        }

        public async Task<Appointment> UpdateOnPatientRequest(DateTime newStart, Appointment appointment)
        {
            var newEnd = newStart + (appointment.EndDateTime - appointment.StartDateTime);

            // The room and doctor are not available in the selected period
            if ((await _roomService.GetAllFree(newStart, newEnd)).ToList().All(room => room.ID != appointment.Room.ID) ||
                !await _doctorService.IsDoctorFreeInTimeSpan(newStart, newEnd, appointment.Doctor))
            {
                return null;
            }

            appointment.StartDateTime = newStart;
            appointment.EndDateTime = newEnd;
            await Update(appointment);
            return appointment;
        }

        public async Task<Appointment> CreateOnPatientRequest(TimeSpan preferredStartTime, TimeSpan preferredEndTime, DateTime latestAcceptableDate,
            Patient requestingPatient, Doctor requestedDoctor, AppointmentPriority appointmentPriority)
        {
            Room room = null;
            Doctor doctor = null;
            var start = DateTime.Now;
            var end = DateTime.Now;


            if (preferredStartTime < requestedDoctor.WorkingHoursStart)
            {
                preferredStartTime = requestedDoctor.WorkingHoursStart;
            }

            if (preferredEndTime > requestedDoctor.WorkingHoursEnd)
            {
                preferredEndTime = requestedDoctor.WorkingHoursEnd;
            }

            switch (appointmentPriority)
            {
                case AppointmentPriority.Doctor:
                    var (freeRoom, timeSlotStart) = await _roomService.GetFirstFreeByTimeSlotAndDoctor(preferredStartTime, preferredEndTime, latestAcceptableDate, requestedDoctor);
                    
                    if (freeRoom != null && timeSlotStart != null)
                    {
                        room = freeRoom;
                        start = timeSlotStart.Value;
                        end = start + TimeSpan.FromMinutes(29);
                        doctor = requestedDoctor;
                    }
                    else
                    {
                        goto case AppointmentPriority.Time;
                    }
                    break;

                case AppointmentPriority.Time:
                    var allDoctors = await _doctorService.GetAll();
                    foreach (var doc in allDoctors.ToList())
                    {
                        (freeRoom, timeSlotStart) = await _roomService.GetFirstFreeByTimeSlotAndDoctor(preferredStartTime, preferredEndTime, latestAcceptableDate, requestedDoctor);
                        if (freeRoom == null || timeSlotStart == null) continue;
                        room = freeRoom;
                        start = timeSlotStart.Value;
                        end = start + TimeSpan.FromMinutes(29);
                        doctor = doc;
                        break;
                    }
                    if (room == null || doctor == null)
                        return null;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(appointmentPriority), appointmentPriority, null);
            }

            var patientsAppointments = await GetAllByPatient(requestingPatient);
            if (patientsAppointments.Any(a => (a.StartDateTime >= start && a.StartDateTime <= end) ||
                                              (a.EndDateTime >= start && a.EndDateTime <= end) ||
                                              (a.StartDateTime >= start && a.EndDateTime <= end)))
            {
                return null;
            }

            var appointment = new Appointment();
            appointment = await Create(appointment);
            appointment.IsActive = true;
            appointment.StartDateTime = start;
            appointment.EndDateTime = end;
            appointment.Doctor = doctor;
            appointment.Patient = requestingPatient;
            appointment.Room = room;
            appointment.Status = AppointmentStatus.Scheduled;
            appointment.Type = doctor?.Specializations.FirstOrDefault();

            return await Update(appointment);
        }

        public async Task<AppointmentRequest> CreateAppointmentRequest(DateTime start, DateTime end, Patient patient, Doctor requester, Doctor proposedDoctor, DateTime timestamp, Room room)
        {
            var appointmentRequest = new AppointmentRequest();

            await using var context = ContextFactory.CreateDbContext();
            var createdAppointmentRequest = (await context.AppointmentRequests.AddAsync(appointmentRequest)).Entity;
            await context.SaveChangesAsync();

            createdAppointmentRequest.IsActive = true;
            createdAppointmentRequest.Room = room;
            createdAppointmentRequest.StartDate = start;
            createdAppointmentRequest.EndDate = end;
            createdAppointmentRequest.IsApproved = false;
            createdAppointmentRequest.Patient = patient;
            createdAppointmentRequest.Requester = requester;
            createdAppointmentRequest.ProposedDoctor = proposedDoctor;

            await using var context1 = ContextFactory.CreateDbContext();
            context.AppointmentRequests.Update(appointmentRequest);
            await context.SaveChangesAsync();

            await _notificationService.PublishAppointmentRequestNotification(appointmentRequest, timestamp, string.Empty);

            return createdAppointmentRequest;
        }

        public async Task<AppointmentChangeRequest> CreateAppointmentChangeRequest(DateTime start, DateTime end, Appointment appointment, DateTime timestamp, string message)
        {

            var appointmentChangeRequest = new AppointmentChangeRequest();

            await using var context = ContextFactory.CreateDbContext();
            var createdAppointmentChangeRequest = (await context.AppointmentChangeRequests.AddAsync(appointmentChangeRequest)).Entity;
            await context.SaveChangesAsync();

            createdAppointmentChangeRequest.IsActive = true;
            createdAppointmentChangeRequest.NewStartDateTime = start;
            createdAppointmentChangeRequest.NewEndDateTime = end;
            createdAppointmentChangeRequest.IsApproved = false;
            createdAppointmentChangeRequest.Appointment = appointment;
            createdAppointmentChangeRequest.PreviousStartDateTime = appointment.StartDateTime;
            createdAppointmentChangeRequest.PreviousEndDateTime = appointment.EndDateTime;

            await using var context1 = ContextFactory.CreateDbContext();
            context.AppointmentChangeRequests.Update(createdAppointmentChangeRequest);
            await context.SaveChangesAsync();


            await _notificationService.PublishAppointmentChangeRequestNotification(appointmentChangeRequest, timestamp, message);

            return createdAppointmentChangeRequest;
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