using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.UserServices;

namespace HospitalCalendar.WPF.ViewModels.DoctorMenu
{
    public class AnamnesisUpdateViewModel : ViewModelBase
    {
        public Doctor Doctor { get; set; }

        public AnamnesisUpdateViewModel(IDoctorService doctorService, IPatientService patientService)
        {
        }
    }
}
