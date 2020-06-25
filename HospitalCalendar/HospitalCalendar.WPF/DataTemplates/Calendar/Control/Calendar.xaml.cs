using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HospitalCalendar.WPF.DataTemplates.Calendar.Control
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : UserControl
    {
        public Calendar()
        {
            InitializeComponent();
        }

        private bool _isDown;
        private void Border_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDown = true;
        }
        private void Border_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDown)
            {
                _isDown = false;
                Keyboard.Focus(sender as Border);
            }
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            var timeLineEvent = (TimeLineEvent)((Border)sender).DataContext;
            if (timeLineEvent.EventUnselected.CanExecute(timeLineEvent.CalendarEntry.CalendarEntry))
            {
                timeLineEvent.EventUnselected.Execute(timeLineEvent.CalendarEntry.CalendarEntry);
            }
        }
    }
}
