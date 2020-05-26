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
using HospitalCalendar.WPF.ViewModels.AdministratorMenu;

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
