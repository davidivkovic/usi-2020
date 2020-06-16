using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services.UserServices;
using HospitalCalendar.EntityFramework.Exceptions;
using HospitalCalendar.WPF.Messages;

namespace HospitalCalendar.WPF.ViewModels.SecretaryMenu
{
    public class PatientRegisterViewModel : ViewModelBase
    {
        private readonly IPatientService _patientService;
        public ICommand RegisterPatient { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InsuranceNumber { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool ValidationError { get; set; }
        public bool UsernameAlreadyExists { get; set; }
        public bool NonMatchingPasswords { get; set; }
        public IEnumerable<Sex> Sexes { get; set; }
        public Sex SelectedSex { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }

        public PatientRegisterViewModel(IPatientService patientService)
        {
            _patientService = patientService;
            Sexes = Enum.GetValues(typeof(Sex)).Cast<Sex>();
            RegisterPatient = new RelayCommand(ExecuteRegistration);
            SelectedSex = Sexes.First();
        }
        public async void Initialize()
        {
            await LoadPatients();
        }

        private async void ExecuteRegistration()
        {
            var inputFields = new List<string> { FirstName, LastName, Username, Password, ConfirmPassword };

            ValidationError = false;
            UsernameAlreadyExists = false;
            NonMatchingPasswords = false;

            try
            {
                if (inputFields.Any(string.IsNullOrWhiteSpace))
                {
                    throw new ArgumentNullException();
                }

                if (Password != ConfirmPassword)
                {
                    throw new NonMatchingPasswordException(Password);
                }

                var registeredUser = await _patientService.Register(FirstName, LastName, Username, Password, SelectedSex, InsuranceNumber);
                Patients.Insert(0, registeredUser);
                Clear();
            }
            catch (ArgumentNullException)
            {
                ValidationError = true;
            }
            catch (NonMatchingPasswordException)
            {
                NonMatchingPasswords = true;
            }
            catch (UsernameAlreadyExistsException)
            {
                UsernameAlreadyExists = true;
            }
        }
        private void Clear()
        {
            FirstName = LastName = Username = InsuranceNumber = Password = ConfirmPassword = string.Empty;
        }

        private async Task LoadPatients()
        {
            var allPatients = await _patientService.GetAll();
            Patients = new ObservableCollection<Patient>(allPatients);
        }
    }
}
