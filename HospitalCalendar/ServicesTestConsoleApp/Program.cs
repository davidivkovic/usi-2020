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
using System.Collections.Generic;
using System.Linq;
using HospitalCalendar.Domain.Services.CalendarEntryServices;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.EntityFramework.Services.CalendarEntryServices;

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
            
            var et = SimpleIoc.Default.GetInstance<IEquipmentTypeService>().GetByName("Bed").Result;

            //var lmao = SimpleIoc.Default.GetInstance<IEquipmentItemService>().GetAllByType(et).Result;
            
            
            RenovationService renovationService = new RenovationService(SimpleIoc.Default.GetInstance<HospitalCalendarDbContextFactory>());


            var lel = SimpleIoc.Default.GetInstance<IEquipmentItemService>().GetAllByType(et).Result.ToList();

            /*Renovation renovation = new Renovation()
            {
                //AddedEquipmentItems = lel
            };*/

            //renovationService.Create(renovation).Wait();
            //var sum = renovationService.GetAll().Result.ToList().First();
            //sum.AddedEquipmentItems = lel;
            //renovationService.Update(sum).GetAwaiter().GetResult();

            var addedEquipmentItems = renovationService.GetAll().GetAwaiter().GetResult();


            IEquipmentTypeService equipmentTypeService = SimpleIoc.Default.GetInstance<IEquipmentTypeService>();
            IUserService userService = SimpleIoc.Default.GetInstance<IUserService>();

            //equipmentTypeService.Create("Blyet", "suka desc", 5).GetAwaiter().GetResult();

            //equipmentTypeService.Create( "Syringe", "30ml medical syringe, nothing much to say here.",130).Wait();
            // userService.Register<Administrator>("David", "Ivkovic", "admin", "pw").GetAwaiter().GetResult();

        }
    }
}
