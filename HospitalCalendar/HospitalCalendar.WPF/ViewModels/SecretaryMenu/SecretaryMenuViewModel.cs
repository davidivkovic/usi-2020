using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;
using HospitalCalendar.WPF.ViewModels.DoctorMenu;

namespace HospitalCalendar.WPF.ViewModels.SecretaryMenu
{
    public class SecretaryMenuViewModel : ViewModelBase
    {
        public static Secretary Secretary { get; set; }
        public ViewModelBase CurrentViewModel { get; set; }

        public SecretaryMenuViewModel()
        {
            MessengerInstance.Register<CurrentUser>(this, message => Secretary = message.User as Secretary);
        }
    }
}
