using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace HospitalCalendar.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IsTabStopProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata(false));
            IsTabStopProperty.OverrideMetadata(typeof(TextBox), new FrameworkPropertyMetadata(true));

            SettingsToggle.Focusable = false;

            Loaded += (o, e) =>
            {
                //Height = ContentControl.ActualHeight;
                //Width = ContentControl.ActualWidth;
                SizeToContent = SizeToContent.WidthAndHeight;
            };

            CloseButton.Click += (s, e) => Close();
            MinimizeButton.Click += (s, e) => WindowState = WindowState.Minimized;
        }

        private void SettingsToggle_Checked(object sender, RoutedEventArgs e)
        {
            AnimateDarkModeMenuOpenedStoryBoard.Completed += (o, ea) =>
            {
                DarkModeToggle.Focus();
            };
        }

        private void DarkModeToggle_OnLostFocus(object sender, RoutedEventArgs e)
        {
            SettingsToggle.IsChecked = false;
        }
    }
}
