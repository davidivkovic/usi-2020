using HandlebarsDotNet;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using IronPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using HospitalCalendar.Domain.Enums;
using HospitalCalendar.Domain.Services.UserServices;

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
            var numberOfDays = (end - start).Days;
            var totalTimeOccupied = await TimeOccupiedByRoom(start, end, room);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task<TimeSpan> TotalTimeOccupiedForAllRooms(DateTime start, DateTime end)
        {
            var allRooms = (await _roomService.GetAll()).ToList();
            var totalTimeOccupied = TimeSpan.Zero;
            var tasks = allRooms.Select(room => TimeOccupiedByRoom(start, end, room));
            var results = await Task.WhenAll(tasks);
            results.ToList().ForEach(ts => totalTimeOccupied += ts);
            return totalTimeOccupied;
        }


        public async Task<TimeSpan> AverageDailyOccupiedTimeForAllRooms(DateTime start, DateTime end)
        {
            var numberOfDays = (end - start).Days;
            var totalTimeOccupied = await TotalTimeOccupiedForAllRooms(start, end);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task<TimeSpan> TimeOccupiedByDoctor(DateTime start, DateTime end, Doctor doctor)
        {
            var appointmentsInTimeFrame = (await _appointmentService.GetAllByTimeFrame(start, end)).Where(appointment => appointment.Doctor.ID == doctor.ID).ToList();
            var totalTimeOccupied = appointmentsInTimeFrame.Aggregate(TimeSpan.Zero, (timeSpan, appointment) => timeSpan.Add(appointment.EndDateTime - appointment.StartDateTime));
            return totalTimeOccupied;
        }

        public async Task<TimeSpan> AverageDailyOccupiedTimeByDoctor(DateTime start, DateTime end, Doctor doctor)
        {
            var numberOfDays = (end - start).Days;
            var totalTimeOccupied = await TimeOccupiedByDoctor(start, end, doctor);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task<TimeSpan> TotalTimeOccupiedForAllDoctors(DateTime start, DateTime end)
        {
            var allDoctors = (await _doctorService.GetAll()).ToList();
            var totalTimeOccupied = TimeSpan.Zero;
            var tasks = allDoctors.Select(doctor => TimeOccupiedByDoctor(start, end, doctor));
            var results = await Task.WhenAll(tasks);
            results.ToList().ForEach(ts => totalTimeOccupied += ts);
            return totalTimeOccupied;
        }

        public async Task<TimeSpan> AverageDailyOccupiedTimeForAllDoctors(DateTime start, DateTime end)
        {
            var numberOfDays = (end - start).Days;
            var totalTimeOccupied = await TotalTimeOccupiedForAllDoctors(start, end);
            return TimeSpan.FromMinutes(totalTimeOccupied.TotalMinutes / numberOfDays);
        }

        public async Task GenerateRoomReport(DateTime start, DateTime end, string directory, FileFormat format)
        {
            var allRooms = (await _roomService.GetAll()).ToList();
            var outputHtml = await InsertRoomDataIntoHtml(start, end, allRooms);
            var html = await File.ReadAllTextAsync(@"Services\Assets\room.html");
            var totalTimeAll = await TotalTimeOccupiedForAllRooms(start, end);
            var averageTimeAll = await AverageDailyOccupiedTimeForAllRooms(start, end);

            var headerData = new
            {
                startDate = start.ToString("dd.MM.yyyy."),
                endDate = end.ToString("dd.MM.yyyy."),
                total = CustomTimeSpanFormat(totalTimeAll),
                averageOverall = CustomTimeSpanFormat(averageTimeAll)
            };

            var stringBuilder = new StringBuilder(html);

            html = stringBuilder.Replace("{{startDate}}", headerData.startDate)
                .Replace("{{endDate}}", headerData.endDate)
                .Replace("{{total}}", headerData.total)
                .Replace("{{averageOverall}}", headerData.averageOverall)
                .Replace("<!--CONTENT-->", outputHtml)
                .ToString();

            var renderer = new HtmlToPdf
            {
                PrintOptions = {MarginTop = 0, MarginBottom = 0, MarginLeft = 0, MarginRight = 0}
            };

            await Task.Run(() => {
                switch (format)
                {
                    case FileFormat.Pdf:
                        var pdf = renderer.RenderHtmlAsPdf(html);
                        pdf.SaveAs(directory + @"\RoomReport.pdf");
                        break;
                    case FileFormat.Csv:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            });
        }

        private async Task<string> InsertRoomDataIntoHtml(DateTime start, DateTime end, IReadOnlyCollection<Room> allRooms)
        {
            const string rowTemplate = "<ul><li>{{floor}}</li><li>{{number}}</li><li>{{purpose}}</li><li>{{total}}</li><li>{{average}}</li></ul>";
            var template = Handlebars.Compile(rowTemplate);
            var outputHtml = string.Empty;

            await Task.Run(async () =>
            {
                foreach (var room in allRooms)
                {
                    var totalTime = await TimeOccupiedByRoom(start, end, room);
                    var averageTime = await AverageDailyOccupiedTimeByRoom(start, end, room);

                    var data = new
                    {
                        floor = room.Floor.ToString(),
                        number = room.Number,
                        purpose = room.Type.ToString(),
                        total = CustomTimeSpanFormat(totalTime),
                        average = CustomTimeSpanFormat(averageTime)
                    };
                    outputHtml += template(data);
                }
            });
            return outputHtml;
        }

        public async Task GenerateDoctorReport(DateTime start, DateTime end, string directory, FileFormat format)
        {
            var allDoctors = (await _doctorService.GetAll()).ToList();
            var outputHtml = await InsertDoctorDataIntoHtml(start, end, allDoctors);

            var html = await File.ReadAllTextAsync(@"Services\Assets\doctor.html");
            var totalTimeAll = await TotalTimeOccupiedForAllDoctors(start, end);
            var averageTimeAll = await AverageDailyOccupiedTimeForAllDoctors(start, end);

            var headerData = new
            {
                startDate = start.ToString("dd.MM.yyyy."),
                endDate = end.ToString("dd.MM.yyyy."),
                total = CustomTimeSpanFormat(totalTimeAll),
                averageOverall = CustomTimeSpanFormat(averageTimeAll)
            };

            var stringBuilder = new StringBuilder(html);

            html = stringBuilder.Replace("{{startDate}}", headerData.startDate)
                .Replace("{{endDate}}", headerData.endDate)
                .Replace("{{total}}", headerData.total)
                .Replace("{{averageOverall}}", headerData.averageOverall)
                .Replace("<!--CONTENT-->", outputHtml)
                .ToString();

            var renderer = new HtmlToPdf
            {
                PrintOptions = { MarginTop = 0, MarginBottom = 0, MarginLeft = 0, MarginRight = 0 }
            };

            await Task.Run(() => {
                switch (format)
                {
                    case FileFormat.Pdf:
                        var pdf = renderer.RenderHtmlAsPdf(html);
                        pdf.SaveAs(directory + @"\DoctorReport.pdf");
                        break;
                    case FileFormat.Csv:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            });
        }

        private async Task<string> InsertDoctorDataIntoHtml(DateTime start, DateTime end, IReadOnlyCollection<Doctor> allDoctors)
        {
            const string rowTemplate = "<ul><li>{{firstName}}</li><li>{{lastName}}</li><li>{{total}}</li><li>{{average}}</li></ul>";
            var template = Handlebars.Compile(rowTemplate);
            var outputHtml = string.Empty;
            await Task.Run(async () =>
            {
                foreach (var doctor in allDoctors)
                {
                    var totalTime = await TimeOccupiedByDoctor(start, end, doctor);
                    var averageTime = await AverageDailyOccupiedTimeByDoctor(start, end, doctor);

                    var data = new
                    {
                        firstName = doctor.FirstName,
                        lastName = doctor.LastName,
                        total = CustomTimeSpanFormat(totalTime),
                        average = CustomTimeSpanFormat(averageTime)
                    };
                    outputHtml += template(data);
                }
            });
            return outputHtml;
        }

        public async Task GeneratePersonalDoctorReport(Doctor doctor, DateTime start, DateTime end, string directory, FileFormat format)
        {
            var totalTime = await TimeOccupiedByDoctor(start, end, doctor);
            var averageTime = await AverageDailyOccupiedTimeByDoctor(start, end, doctor);

            var headerData = new
            {
                startDate = start.ToString("dd.MM.yyyy."),
                endDate = end.ToString("dd.MM.yyyy."),
                total = CustomTimeSpanFormat(totalTime),
                averageOverall = CustomTimeSpanFormat(averageTime)
            };

            var html = await File.ReadAllTextAsync(@"Services\Assets\doctorHimself.html");
            var stringBuilder = new StringBuilder(html);
            var outputHtml = await InsertPersonalDoctorDataIntoHtml(start, end, doctor);

            html = stringBuilder
                .Replace("{{doctorName}}", doctor.FirstName + " " + doctor.LastName)
                .Replace("{{startDate}}", headerData.startDate)
                .Replace("{{endDate}}", headerData.endDate)
                .Replace("{{total}}", headerData.total)
                .Replace("{{average}}", headerData.averageOverall)
                .Replace("/*CHART_DATA*/", outputHtml)
                .ToString();
            
            var renderer = new HtmlToPdf
            {
                PrintOptions = { MarginTop = 0, MarginBottom = 0, MarginLeft = 0, MarginRight = 0 }
            };

            renderer.PrintOptions.EnableJavaScript = true;
            renderer.PrintOptions.RenderDelay = 500; //milliseconds

            await Task.Run(() => {
                switch (format)
                {
                    case FileFormat.Pdf:
                        var pdf = renderer.RenderHtmlAsPdf(html);
                        pdf.SaveAs(directory + @"\PersonalDoctorReport.pdf");
                        break;
                    case FileFormat.Csv:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(format), format, null);
                }
            });
        }

        private async Task<string> InsertPersonalDoctorDataIntoHtml(DateTime start, DateTime end, Doctor doctor)
        {
            const string rowTemplate = ", ['{{date}}', {{time}}]";
            var template = Handlebars.Compile(rowTemplate);
            var outputHtml = string.Empty;

            await Task.Run(async () =>
            {
                while (start < end)
                {
                    var totalHours = (await TimeOccupiedByDoctor(start, end + TimeSpan.FromDays(1), doctor)).TotalHours;

                    var data = new
                    {
                        date = start.ToString("dd.MM.yyyy."),
                        time = totalHours
                    };
                    outputHtml += template(data);

                    start += TimeSpan.FromDays(1);
                }
            });
            return outputHtml;
        }

        private static string CustomTimeSpanFormat(TimeSpan input) => $"{(int) input.TotalHours:D2}H {(int) (input.TotalMinutes % 60):D2}m";
    }
}