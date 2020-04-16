using HospitalCalendar.EntityFramework.Services;
using System;
using System.Collections.Generic;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.EntityFramework;
using System.Linq;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.EntityFramework.Services.AuthenticationServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;

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


            SimpleIoc.Default.Register<IUserService, UserDataService>();
            SimpleIoc.Default.Register<IPasswordHasher<User>, PasswordHasher<User>>();

            // The Authentication service takes parameters AuthenticationService(IUserService userService, IPasswordHasher<User> passwordHasher)
            // We register those interfaces and their implementations as services above

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();




            SimpleIoc.Default.Register<IDataService<DomainObject>, GenericDataService<DomainObject>>();

            IDataService<Doctor> doctorService = SimpleIoc.Default.GetInstance<IDataService<Doctor>>();


            HospitalCalendarDbContextFactory dbContext = SimpleIoc.Default.GetInstance<HospitalCalendarDbContextFactory>();
            HospitalCalendarDbContext hospitalCalendarDbContext = dbContext.CreateDbContext();


            IAuthenticationService authenticationService = SimpleIoc.Default.GetInstance<IAuthenticationService>();


            // administrator registers a doctor
            // returns an element of the RegistrationResultEnum
            var register = authenticationService.Register<Doctor>("Name", "LastName", "username", "password",
                "password").Result;

            Console.WriteLine(register);
            

            // returns the logged in user
            var login = authenticationService.Login("username", "password").Result;

            Console.WriteLine(login);

            // Example of finding doctors
            ICollection<Doctor> doctors = new List<Doctor>(doctorService.GetAll().Result);


            // Another example of finding doctors returns a doctor
            var foundDoctor =  hospitalCalendarDbContext
                                                    .Doctors
                                                    .Where(d => d.FirstName == "Name")
                                                    .FirstOrDefaultAsync()
                                                    .Result;


            // Manager later sets the doctors full data

            List<Specialization> specializations = new List<Specialization>();

            specializations.Add(new Specialization()
            {
                SingleSpecialization = Specializations.Anesthesiology,
            });

            specializations.Add(new Specialization()
            {
                SingleSpecialization = Specializations.GeneralSurgery
            });


            foundDoctor.Specializations = specializations;

            foundDoctor.WorkingHoursStart = new TimeSpan(9, 30, 00);
            foundDoctor.WorkingHoursEnd = new TimeSpan(17, 30, 00);
            foundDoctor.DoctorsPatients = new List<DoctorPatient>();


            // We can update database objects like this
            var x = doctorService.Update(foundDoctor).Result;
            

           
            
            
        }
    }
}
