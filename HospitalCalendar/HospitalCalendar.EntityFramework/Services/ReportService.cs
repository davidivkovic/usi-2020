using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
<<<<<<< HEAD

namespace HospitalCalendar.EntityFramework.Services
{
    public class ReportService:GenericDataService<CalendarEntry>,IReportService
    {
        public ReportService(HospitalCalendarDbContextFactory contextFactory) : base(contextFactory)
        {
        }


        public async Task<bool> GenerateRoomReport(DateTime start, DateTime end) 
        {
            List<string> Rooms = new List<string>();
            List<int> Minutes = new List<int>();
            List<double> AverageHoursOfUsagePerRoom = new List<double>();

            List<Appointment> entrys = new List<Appointment>();
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                entrys =await context.Appointments
                                .Where(a => a.IsActive && a.Status==AppointmentStatus.Finished)
                                .Where(a=> a.StartDateTime>=start && a.EndDateTime<=end)
                                .ToListAsync();
            }

            


            for (int i = 0; i < entrys.Count; i++)
            {
                if (!Rooms.Contains(Get_Room_Number(entrys[i].Room)))
                {
                    Rooms.Add(Get_Room_Number(entrys[i].Room));
                    Minutes.Add(0);
                    AverageHoursOfUsagePerRoom.Add(0);
                }
                
                    int index = Rooms.IndexOf(Get_Room_Number(entrys[i].Room));
                    Minutes[index] += Convert.ToInt32(
                        ((TimeSpan)(entrys[i].EndDateTime - entrys[i].StartDateTime))
                        .TotalMinutes);

                    AverageHoursOfUsagePerRoom[index] =
                        (Minutes[index]/60) 
                        /
                        ((TimeSpan)(entrys[i].EndDateTime - entrys[i].StartDateTime)).TotalHours;
                
            }


            double TotalHoursOfUsage = Minutes.Sum() / 60;
            double AverageHoursOfUsage = TotalHoursOfUsage / ((TimeSpan)(end - start)).TotalDays;
            


            return false;
        }

        public async Task<bool> GenerateDoctorReport(DateTime start, DateTime end) 
        {
            List<string> Doctors = new List<string>();
            List<int> Minutes = new List<int>();
            List<double> AverageHoursOfUsagePerDoctor = new List<double>();

            List<Appointment> entrys = new List<Appointment>();
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                entrys = await context.Appointments
                                .Where(a => a.IsActive && a.Status == AppointmentStatus.Finished)
                                .Where(a => a.StartDateTime >= start && a.EndDateTime <= end)
                                .ToListAsync();
            }




            for (int i = 0; i < entrys.Count; i++)
            {
                if (!Doctors.Contains(Get_Doctor_Name(entrys[i].Doctor)))
                {
                    Doctors.Add(Get_Doctor_Name(entrys[i].Doctor));
                    Minutes.Add(0);
                    AverageHoursOfUsagePerDoctor.Add(0);
                }
                
                    int index = Doctors.IndexOf(Get_Doctor_Name(entrys[i].Doctor));
                    Minutes[index] += Convert.ToInt32(
                        ((TimeSpan)(entrys[i].EndDateTime - entrys[i].StartDateTime))
                        .TotalMinutes);

                    AverageHoursOfUsagePerDoctor[index] =
                        (Minutes[index] / 60)
                        /
                        ((TimeSpan)(entrys[i].EndDateTime - entrys[i].StartDateTime)).TotalHours;
                
            }


            double TotalHoursOfUsage = Minutes.Sum() / 60;
            double AverageHoursOfUsage = TotalHoursOfUsage / ((TimeSpan)(end - start)).TotalDays;


            return false;
        }

        public async Task<bool> GenerateDoctorWorkReport(Doctor doctor)
        {
            List<Appointment> entrys = new List<Appointment>();
            int CanceledAppointments = 0;
            int FinishedAppointments = 0;
            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                entrys = await context.Appointments
                                .Where(a => a.IsActive && a.Status == AppointmentStatus.Finished)
                                .Where(a=> a.Doctor.ID == doctor.ID)
                                .ToListAsync();

                

                CanceledAppointments = await context.Appointments
                                        .Where(a => a.IsActive && a.Status == AppointmentStatus.Cancelled)
                                        .Where(a => a.Doctor.ID == doctor.ID)
                                        .CountAsync();
            }

            FinishedAppointments = entrys.Count;
            double CanceledAppointmentsPct = (CanceledAppointments*100) / (CanceledAppointments + FinishedAppointments);
            DateTime FirstAppointment = new DateTime(1970, 1, 1);
            DateTime LastAppointment = DateTime.Now.Date;

            double TotalHours = 0;

            for (int i = 0; i < entrys.Count; i++)
            {
                TotalHours += ((TimeSpan)(entrys[i].EndDateTime - entrys[i].StartDateTime)).TotalHours;
                if (entrys[i].StartDateTime>FirstAppointment)
                {
                    FirstAppointment = entrys[i].StartDateTime;
                }
                if (entrys[i].EndDateTime<LastAppointment)
                {
                    LastAppointment = entrys[i].EndDateTime;
                }
            }

            double AverageHoursPerDay = TotalHours / ((TimeSpan)(LastAppointment - FirstAppointment)).TotalHours;



            return false;
        }

        public async Task<bool> GenerateDoctorPatientReport(Doctor doctor)
        {
            List<Appointment> entrys = new List<Appointment>();

            using (HospitalCalendarDbContext context = ContextFactory.CreateDbContext())
            {
                entrys = await context.Appointments
                                      .Where(a => a.IsActive && a.Doctor.ID == doctor.ID)
                                      .ToListAsync();
            }

            List<string> Patients = new List<string>();
            List<int> nFinishedAppointments = new List<int>();
            List<int> nCancelledAppointmets = new List<int>();
            List<double> TimeInHospital = new List<double>();

            for (int i = 0; i < entrys.Count; i++)
            {
                if (!Patients.Contains(Get_Patient_Name(entrys[i].Patient)))
                {
                    Patients.Add(Get_Patient_Name(entrys[i].Patient));
                    nFinishedAppointments.Add(0);
                    nCancelledAppointmets.Add(0);
                    TimeInHospital.Add(0);
                }

                int index = Patients.IndexOf(Get_Patient_Name(entrys[i].Patient));
                if (entrys[i].Status == AppointmentStatus.Cancelled)
                {
                    nCancelledAppointmets[index] += 1;
                }
                else
                {
                    nFinishedAppointments[index] += 1;
                    TimeInHospital[index] += ((TimeSpan)(entrys[i].EndDateTime - entrys[i].StartDateTime)).TotalHours;
                }


            }



            return false;
        }




        private string Get_Room_Number(Room r) 
        {
            return r.Floor.ToString() + '-' + r.Number.ToString();
        }
        private string Get_Doctor_Name(Doctor d) 
        {
            return d.FirstName + " " + d.LastName;
        }
        private string Get_Patient_Name(Patient p) 
        {
            return p.FirstName + " " + p.LastName;
        }

    }
}
=======
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
>>>>>>> viewmodel-development
