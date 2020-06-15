using System.Windows;

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
            SettingsToggle.Focusable = false;

            Loaded += (o, e) =>
            {
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