using System.Windows;
using System.Windows.Controls;

namespace HospitalCalendar.WPF.Views.SecretaryMenu
{
    /// <summary>
    /// Interaction logic for SecretaryMenu.xaml
    /// </summary>
    public partial class SecretaryMenu : UserControl
    {
        public SecretaryMenu()
        {
            InitializeComponent();
        }

        private void MenuToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            Drawer.IsLeftDrawerOpen = !Drawer.IsLeftDrawerOpen;
        }
    }
}
