using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using HospitalCalendar.Domain.Services.CalendarEntryServices;

namespace HospitalCalendar.EntityFramework.Services
{
    public class ReportService : IReportService
    {
        private readonly ICalendarEntryService _calendarEntryService;
        private readonly IRoomService _roomService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public ReportService(ICalendarEntryService calendarEntryService, IAppointmentService appointmentService, IRoomService roomService, IDoctorService doctorService)
        {
            _calendarEntryService = calendarEntryService;
            _roomService = roomService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        public async Task<TimeSpan> TimeOccupiedByRoom(DateTime start, DateTime end, Room room)
        {
            var calendarEntriesInTimeFrame = (await _calendarEntryService.GetAllByRoomAndTimeFrame(room, start, end)).ToList();
            var totalTimeOccupied = calendarEntriesInTimeFrame.Aggregate(TimeSpan.Zero, (ts, ce) => ts.Add(ce.EndDateTime - ce.StartDateTime));
            return totalTimeOccupied;
        }

        public async Task<TimeSpan> AverageDailyOccupiedTimeByRoom(DateTime start, DateTime end, Room room)
        {
            var numberOfDays = (start - end).Days;
            var totalTimeOccupied = await TimeOccupiedByRoom(start, end, room);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task<TimeSpan> TotalTimeOccupiedForAllRooms(DateTime start, DateTime end)
        {
            var allRooms = (await _roomService.GetAll()).ToList();
            var totalTimeOccupied = TimeSpan.Zero;
            allRooms.ForEach(async room => totalTimeOccupied += await TimeOccupiedByRoom(start, end, room));
            return totalTimeOccupied;
        }

        public async Task<TimeSpan> AverageDailyOccupiedTimeForAllRooms(DateTime start, DateTime end)
        {
            var numberOfDays = (start - end).Days;
            var totalTimeOccupied = await TotalTimeOccupiedForAllRooms(start, end);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task<TimeSpan> TimeOccupiedByDoctor(DateTime start, DateTime end, Doctor doctor)
        {
            var appointmentsInTimeFrame = (await _appointmentService.GetAllByTimeFrame(start, end)).Where(a => a.Doctor.ID == doctor.ID).ToList();
            var totalTimeOccupied = appointmentsInTimeFrame.Aggregate(TimeSpan.Zero, (ts, ce) => ts.Add(ce.EndDateTime - ce.StartDateTime));
            return totalTimeOccupied;
        }

        public async Task<TimeSpan> AverageDailyOccupiedTimeByDoctor(DateTime start, DateTime end, Doctor doctor)
        {
            var numberOfDays = (start - end).Days;
            var totalTimeOccupied = await TimeOccupiedByDoctor(start, end, doctor);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task<TimeSpan> TotalTimeOccupiedForAllDoctors(DateTime start, DateTime end)
        {
            var allDoctors = (await _doctorService.GetAll()).ToList();
            var totalTimeOccupied = TimeSpan.Zero;
            allDoctors.ForEach(async doctor => totalTimeOccupied += await TimeOccupiedByDoctor(start, end, doctor));
            return totalTimeOccupied;
        }

        public async Task<TimeSpan> AverageDailyOccupiedTimeForAllDoctors(DateTime start, DateTime end)
        {
            var numberOfDays = (start - end).Days;
            var totalTimeOccupied = await TotalTimeOccupiedForAllDoctors(start, end);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }
    }
}