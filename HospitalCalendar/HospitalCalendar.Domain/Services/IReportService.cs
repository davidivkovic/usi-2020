using HospitalCalendar.Domain.Models;
using System;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Enums;

namespace HospitalCalendar.Domain.Services
{
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
        Task GenerateRoomReport(DateTime start, DateTime end, string directory, FileFormat format);
        Task GenerateDoctorReport(DateTime start, DateTime end, string directory, FileFormat format);
        Task GeneratePersonalDoctorReport(Doctor doctor, DateTime start, DateTime end, string directory, FileFormat format);
    }
}