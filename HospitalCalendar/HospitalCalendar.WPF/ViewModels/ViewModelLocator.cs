using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Services.AuthenticationServices;
using HospitalCalendar.EntityFramework.Services;
using HospitalCalendar.EntityFramework.Services.AuthenticationServices;
using HospitalCalendar.WPF.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace HospitalCalendar.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // Register services here

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();

            SimpleIoc.Default.Register<IAuthenticationService, AuthenticationService>();

        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        
        
    }
}
