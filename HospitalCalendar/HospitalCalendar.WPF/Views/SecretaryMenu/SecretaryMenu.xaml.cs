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
