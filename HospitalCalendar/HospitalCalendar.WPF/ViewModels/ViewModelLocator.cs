﻿using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.EntityFramework;
using HospitalCalendar.EntityFramework.Services;
using HospitalCalendar.EntityFramework.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework.Services.CalendarEntryServices;
using HospitalCalendar.EntityFramework.Services.EquipmentServices;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.DoctorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using HospitalCalendar.WPF.ViewModels.ManagerMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.DoctorSpecializationsMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.ReportMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;
using HospitalCalendar.WPF.ViewModels.SecretaryMenu;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using HospitalCalendar.Domain.Services.NotificationsServices;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.EntityFramework.Services.NotificationServices;


namespace HospitalCalendar.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ViewModelLocator()
        {
            ServiceProvider = CreateServiceProvider();
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            // Password hasher dependencies
            services.AddScoped<IOptions<PasswordHasherOptions>, OptionsWrapper<PasswordHasherOptions>>();
            services.AddScoped<PasswordHasherOptions>();

            // Password hasher
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Register services here
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<ICalendarEntryService, CalendarEntryService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IRenovationService, RenovationService>();
            services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
            services.AddScoped<IEquipmentItemService, EquipmentItemService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ISurgeryService, SurgeryService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAnamnesisService, AnamnesisService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<HospitalCalendarDbContextFactory>();

            // Register ViewModels here
            services.AddScoped<MainViewModel>();
            services.AddScoped<LoginViewModel>();
            services.AddScoped<UserRegisterViewModel>();
            services.AddScoped<UserUpdateViewModel>();
            services.AddScoped<RoomCreateViewModel>();
            services.AddTransient<AdministratorViewModel>();
            services.AddTransient<ManagerMenuViewModel>();
            services.AddTransient<DoctorMenuViewModel>();
            services.AddTransient<SecretaryMenuViewModel>();
            services.AddScoped<EquipmentMenuViewModel>();
            services.AddScoped<EquipmentTypeCreateViewModel>();
            services.AddScoped<EquipmentTypeUpdateViewModel>();
            services.AddScoped<RenovationMenuViewModel>();
            services.AddScoped<ManagerReportMenuViewModel>();
            services.AddScoped<RoomSearchViewModel>();
            services.AddScoped<DoctorSpecializationsViewModel>();
            services.AddScoped<AppointmentScheduleViewModel>();
            services.AddScoped<AppointmentCreateViewModel>();
            services.AddScoped<AnamnesisUpdateViewModel>();
            services.AddScoped<DoctorReportMenuViewModel>();

            return services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => ServiceProvider.GetRequiredService<MainViewModel>();
        public LoginViewModel LoginViewModel => ServiceProvider.GetRequiredService<LoginViewModel>();
        public AdministratorViewModel AdministratorViewModel => ServiceProvider.GetRequiredService<AdministratorViewModel>();
        public UserRegisterViewModel RegistrationViewModel => ServiceProvider.GetRequiredService<UserRegisterViewModel>();
        public UserUpdateViewModel UserModificationViewModel => ServiceProvider.GetRequiredService<UserUpdateViewModel>();
        public RoomCreateViewModel RoomCreateViewModel => ServiceProvider.GetRequiredService<RoomCreateViewModel>();
        public ManagerMenuViewModel ManagerMenuViewModel => ServiceProvider.GetRequiredService<ManagerMenuViewModel>();
        public EquipmentMenuViewModel EquipmentMenuViewModel => ServiceProvider.GetRequiredService<EquipmentMenuViewModel>();
        public EquipmentTypeCreateViewModel EquipmentTypeCreateViewModel => ServiceProvider.GetRequiredService<EquipmentTypeCreateViewModel>();
        public EquipmentTypeUpdateViewModel EquipmentTypeUpdateViewModel => ServiceProvider.GetRequiredService<EquipmentTypeUpdateViewModel>();
        public RenovationMenuViewModel RenovationMenuViewModel => ServiceProvider.GetRequiredService<RenovationMenuViewModel>();
        public RoomSearchViewModel RoomSearchViewModel => ServiceProvider.GetRequiredService<RoomSearchViewModel>();
        public DoctorSpecializationsViewModel DoctorSpecializationsViewModel => ServiceProvider.GetRequiredService<DoctorSpecializationsViewModel>();
        public AppointmentScheduleViewModel AppointmentScheduleViewModel => ServiceProvider.GetService<AppointmentScheduleViewModel>();
        public DoctorMenuViewModel DoctorMenuViewModel => ServiceProvider.GetService<DoctorMenuViewModel>();
        public AppointmentCreateViewModel AppointmentCreateViewModel => ServiceProvider.GetRequiredService<AppointmentCreateViewModel>();
        public AnamnesisUpdateViewModel AnamnesisUpdateViewModel => ServiceProvider.GetRequiredService<AnamnesisUpdateViewModel>();
        public SecretaryMenuViewModel SecretaryMenuViewModel => ServiceProvider.GetRequiredService<SecretaryMenuViewModel>();
        public ManagerReportMenuViewModel ManagerReportMenuViewModel => ServiceProvider.GetRequiredService<ManagerReportMenuViewModel>();
        public DoctorReportMenuViewModel DoctorReportMenuViewModel => ServiceProvider.GetRequiredService<DoctorReportMenuViewModel>();
    }
}