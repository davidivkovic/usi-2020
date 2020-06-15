using System.Windows;
using System.Windows.Controls;

namespace HospitalCalendar.WPF.Views.ManagerMenu
{
    /// <summary>
    /// Interaction logic for ManagerMenu.xaml
    /// </summary>
    public partial class ManagerMenu : UserControl
    {
        public ManagerMenu()
        {
            InitializeComponent();
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            Drawer.IsLeftDrawerOpen = !Drawer.IsLeftDrawerOpen;
        }
    }
}
