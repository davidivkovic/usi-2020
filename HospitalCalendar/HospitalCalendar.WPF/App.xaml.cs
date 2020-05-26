using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Ioc;
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
using HospitalCalendar.WPF.ViewModels;
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using HospitalCalendar.WPF.ViewModels.DoctorMenu;
using HospitalCalendar.WPF.ViewModels.Login;
using HospitalCalendar.WPF.ViewModels.ManagerMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.DoctorSpecializationsMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.EquipmentMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RoomSearchMenu;
using HospitalCalendar.WPF.ViewModels.SecretaryMenu;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HospitalCalendar.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
    }
}
