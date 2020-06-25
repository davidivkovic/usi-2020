using HospitalCalendar.WPF.ViewModels.AdministratorMenu;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace HospitalCalendar.WPF.Views.ManagerMenu.RoomSearchMenu
{
    /// <summary>
    /// Interaction logic for RoomList.xaml
    /// </summary>
    public partial class SearchRoomList : UserControl
    {
        public SearchRoomList()
        {
            InitializeComponent();
        }

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(SearchRoomList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(SearchRoomList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable<RoomBindableViewModel> ItemsSource
        {
            get => (IEnumerable<RoomBindableViewModel>)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}
