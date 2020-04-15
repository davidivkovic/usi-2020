using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight;

namespace HospitalCalendar.WPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;

            set
            {
                if (_currentViewModel == value)
                    return;
                _currentViewModel = value;
                RaisePropertyChanged("CurrentViewModel");
            }
        }

        public MainViewModel()
        {

            CurrentViewModel = new LoginViewModel();
        }

    }
}
