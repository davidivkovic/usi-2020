
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;

namespace HospitalCalendar.WPF.ViewModels.PatientMenu
{
    public class AnamnesisViewModel : ViewModelBase
    {
        private readonly IAnamnesisService _anamnesisService;
        public Patient Patient { get; set; }
        public List<Entry> Anamnesis { get; set; }
        public Entry CurrentlySelectedEntry { get; set; }
        public AnamnesisViewModel(IAnamnesisService anamnesisService)
        {
            _anamnesisService = anamnesisService;
            Anamnesis = new List<Entry>();
        }
        public async void Initialize()
        {
            var entries = await _anamnesisService.GetAllByPatient(Patient);
            Anamnesis = new List<Entry>(entries);
        }
    }
}
