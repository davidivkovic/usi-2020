using System.Windows;
using System.Windows.Controls;

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
