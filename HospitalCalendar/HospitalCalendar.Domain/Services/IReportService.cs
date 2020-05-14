using HospitalCalendar.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IReportService:IDataService<CalendarEntry>
    {
        Task<bool> GenerateRoomReport(DateTime start, DateTime end);
        Task<bool> GenerateDoctorReport(DateTime start, DateTime end);
        Task<bool> GenerateDoctorWorkReport(Doctor doctor);
        Task<bool> GenerateDoctorPatientReport(Doctor doctor);
    }
}
