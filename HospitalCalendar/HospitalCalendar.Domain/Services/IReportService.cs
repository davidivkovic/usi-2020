using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
<<<<<<< HEAD
    public interface IReportService:IDataService<CalendarEntry>
    {
        Task<bool> GenerateRoomReport(DateTime start, DateTime end);
        Task<bool> GenerateDoctorReport(DateTime start, DateTime end);
        Task<bool> GenerateDoctorWorkReport(Doctor doctor);
        Task<bool> GenerateDoctorPatientReport(Doctor doctor);
    }
}
=======
    public interface IReportService
    {
        Task<TimeSpan> TimeOccupiedByRoom(DateTime start, DateTime end, Room room);
        Task<TimeSpan> AverageDailyOccupiedTimeByRoom(DateTime start, DateTime end, Room room);
        Task<TimeSpan> TotalTimeOccupiedForAllRooms(DateTime start, DateTime end);
        Task<TimeSpan> AverageDailyOccupiedTimeForAllRooms(DateTime start, DateTime end);
        Task<TimeSpan> TimeOccupiedByDoctor(DateTime start, DateTime end, Doctor doctor);
        Task<TimeSpan> AverageDailyOccupiedTimeByDoctor(DateTime start, DateTime end, Doctor doctor);
        Task<TimeSpan> TotalTimeOccupiedForAllDoctors(DateTime start, DateTime end);
        Task<TimeSpan> AverageDailyOccupiedTimeForAllDoctors(DateTime start, DateTime end);
    }
}
>>>>>>> viewmodel-development
