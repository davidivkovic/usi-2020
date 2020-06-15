using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.Messages;

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
