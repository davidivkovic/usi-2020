using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using HospitalCalendar.Domain.Enums;
using HospitalCalendar.Domain.Models;

namespace HospitalCalendar.WPF.ViewModels.ManagerMenu.ReportMenu
{
    public class ManagerReportMenuViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        public ICommand GenerateReport { get; set; }
        public ICommand ChooseDirectory { get; set; }
        public FileFormat SelectedFileFormat { get; set; } = FileFormat.Pdf;
        public bool IsLoading { get; set; }
        public DateTime? ReportStartDateTime { get; set; } = null;
        public DateTime? ReportEndDateTime { get; set; } = null;
        public string FilePath { get; set; }
        public SnackbarMessageQueue MaterialDesignMessageQueue { get; set; }
        public string GeneratedHtml { get; set; }
        public List<DomainObject> AvailableReportTypes { get; set; } = new List<DomainObject> {new Room(), new Doctor()};
        public object SelectedReportType { get; set; }

        public ManagerReportMenuViewModel(IReportService reportService)
        {
            _reportService = reportService;
            MaterialDesignMessageQueue = new SnackbarMessageQueue(TimeSpan.FromSeconds(2))
            {
                IgnoreDuplicate = true
            };
            GenerateReport = new RelayCommand(ExecuteGenerateReport);
            ChooseDirectory = new RelayCommand(ExecuteChooseDirectory);
            SelectedReportType = AvailableReportTypes.First();
        }

        private static void ExecuteChooseDirectory() { }

        // Method is there to conform to other ViewModels
        // Can be used in the future to load data when the ViewModel is requested
        public void Initialize() { }

        public async void ExecuteGenerateReport()
        {
            if (ReportEndDateTime == null || ReportStartDateTime == null) return;
            var reportFilePath = FilePath;
            MaterialDesignMessageQueue.Enqueue("Generating your report...", true);
            IsLoading = true;

            switch (SelectedReportType)
            {
                case Doctor _:
                    await _reportService.GenerateDoctorReport(ReportStartDateTime.Value, ReportEndDateTime.Value, FilePath, SelectedFileFormat);
                    break;
                case Room _:
                    await _reportService.GenerateRoomReport(ReportStartDateTime.Value, ReportEndDateTime.Value, FilePath, SelectedFileFormat);
                    break;
            }

            IsLoading = false;

            MaterialDesignMessageQueue.Enqueue("Your report has been generated successfully", "Open file location", message =>
            {
                var startInfo = new ProcessStartInfo
                {
                    Arguments = reportFilePath,
                    FileName = "explorer.exe"
                };
                Process.Start(startInfo);
            }, null, false, true, TimeSpan.FromSeconds(6)); ;
        }
    }
}