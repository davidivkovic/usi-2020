using System;
using System.Runtime.InteropServices.ComTypes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using HospitalCalendar.Domain.Models;
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
using HospitalCalendar.WPF.ViewModels.Login;
using HospitalCalendar.WPF.ViewModels.ManagerMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


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

            //SimpleIoc.Default.Unregister<IMessenger>();
            //SimpleIoc.Default.Register(() => Messenger.Default);

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
            services.AddScoped<HospitalCalendarDbContextFactory>();

            // Register ViewModels here
            services.AddScoped<MainViewModel>();
            services.AddScoped<LoginViewModel>();
            services.AddTransient<AdministratorViewModel>();
            services.AddTransient<UserRegisterViewModel>();
            services.AddTransient<UserUpdateViewModel>();
            services.AddTransient<RoomCreateViewModel>();
            services.AddTransient<ManagerMenuViewModel>();
            services.AddTransient<EquipmentMenuViewModel>();
            services.AddTransient<EquipmentTypeCreateViewModel>();
            services.AddTransient<EquipmentTypeUpdateViewModel>();
            services.AddTransient<RenovationMenuViewModel>();
            services.AddTransient<RoomSearchViewModel>();

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
    }
}
