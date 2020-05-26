using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HospitalCalendar.WPF.ViewModels.DoctorMenu;

namespace HospitalCalendar.WPF.Views.DoctorMenu
{
    /// <summary>
    /// Interaction logic for DoctorMenu.xaml
    /// </summary>
    public partial class DoctorMenu : UserControl
    {
        public DoctorMenu()
        {
            InitializeComponent();
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            Drawer.IsLeftDrawerOpen = !Drawer.IsLeftDrawerOpen;
        }
    }
}
