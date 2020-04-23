using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.Domain.Services.EquipmentServices;
using HospitalCalendar.EntityFramework;
using HospitalCalendar.EntityFramework.Services;
using HospitalCalendar.EntityFramework.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework.Services.EquipmentServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is IOC - inversion of control
            // We couple every interface with some of its' implementations

            // Every service will inject its required dependencies
            // We can then simply retrieve the service by its interface
            // Without having to worry about its' required parameters

            SimpleIoc.Default.Register<HospitalCalendarDbContextFactory>();

            // Password hasher dependencies
            SimpleIoc.Default.Register<IOptions<PasswordHasherOptions>, OptionsWrapper<PasswordHasherOptions>>();
            SimpleIoc.Default.Register<PasswordHasherOptions>();

            SimpleIoc.Default.Register<IUserService, UserService>();
            SimpleIoc.Default.Register<IPasswordHasher<User>, PasswordHasher<User>>();

            // The Authentication service takes parameters AuthenticationService(IUserService userService, IPasswordHasher<User> passwordHasher)
            // We register those interfaces and their implementations as services above

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();
            SimpleIoc.Default.Register<IEquipmentTypeService, EquipmentTypeService>();
            SimpleIoc.Default.Register<IEquipmentItemService, EquipmentItemService>();
            SimpleIoc.Default.Register<IRoomService, RoomService>();

            HospitalCalendarDbContextFactory dbContext = SimpleIoc.Default.GetInstance<HospitalCalendarDbContextFactory>();
            HospitalCalendarDbContext hospitalCalendarDbContext = dbContext.CreateDbContext();


            IUserService userService = SimpleIoc.Default.GetInstance<IUserService>();

            var user = userService.Register<Patient>("Name", "Surname", "user", "pw").Result;
            //Console.WriteLine(user.Username);

            //Console.WriteLine(userService.GetByUsername("user1").Result);
            //Console.WriteLine(userService.GetByUsername("user").Result.Username);

            var result = userService.Update(user, "test", "test", "user", "         ").Result;
            Console.WriteLine(userService.GetByUsername("user").Result.FirstName);

            IAuthenticationService authenticationService = SimpleIoc.Default.GetInstance<IAuthenticationService>();

            // Console.WriteLine(authenticationService.Login("user", "pwp").Result);
            Console.WriteLine(authenticationService.Login("user", "pw").Result.Username);


            IEquipmentTypeService equipmentTypeService = SimpleIoc.Default.GetInstance<IEquipmentTypeService>();






        }
    }
}
