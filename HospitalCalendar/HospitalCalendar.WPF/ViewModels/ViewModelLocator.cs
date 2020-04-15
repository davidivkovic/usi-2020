using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.EntityFramework.Services;
using HospitalCalendar.WPF.ViewModels;


namespace HospitalCalendar.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            // Register services here

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();

        }

        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();

        public LoginViewModel LoginViewModel => SimpleIoc.Default.GetInstance<LoginViewModel>();
        
        
    }
}
