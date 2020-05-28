﻿using System;
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

namespace HospitalCalendar.WPF.Views.ManagerMenu.RenovationMenu
{
    /// <summary>
    /// Interaction logic for RoomList.xaml
    /// </summary>
    public partial class RenovationRoomList : UserControl
    {
        public RenovationRoomList()
        {
            InitializeComponent();
        }

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(RenovationRoomList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(RenovationRoomList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable<RoomBindableViewModel> ItemsSource
        {
            get => (IEnumerable<RoomBindableViewModel>)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public event MyComboBoxSelectionChangedEventHandler ComboBoxSelectionChanged;

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (e.AddedItems.Count > 0)
            {
                ComboBoxSelectionChanged?.Invoke(this, new MyComboBoxSelectionChangedEventArgs { ComboBoxItem = e.AddedItems[0] });
            }
        }
    }

    public class MyComboBoxSelectionChangedEventArgs : EventArgs
    {
        public object ComboBoxItem { get; set; }
    }

    public delegate void MyComboBoxSelectionChangedEventHandler(object sender, MyComboBoxSelectionChangedEventArgs e);
}
