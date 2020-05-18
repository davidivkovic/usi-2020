using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
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
using HospitalCalendar.Domain.Models;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.RenovationMenu;
using MaterialDesignThemes.Wpf;

namespace HospitalCalendar.WPF.Views.ManagerMenu.RenovationMenu
{
    /// <summary>
    /// Interaction logic for RenovationMenu.xaml
    /// </summary>
    public partial class RenovationMenu : UserControl
    {
        public Storyboard Storyboard { get; set; }

        public RenovationMenu()
        {
            InitializeComponent();

            FreeEquipmentTypes.EquipmentListBox.ItemsSource = ((RenovationMenuViewModel)DataContext).FreeEquipmentTypes;
            EquipmentTypesInRoom.EquipmentListBox.ItemsSource = ((RenovationMenuViewModel)DataContext).EquipmentTypesInRoom;

            AddEquipmentToRoom.IsEnabled = false;
            RemoveEquipmentFromRoom.IsEnabled = false;
            RoomsAvailableToJoinTo.IsEnabled = false;

            Storyboard = RenovationCalendar.FindResource("AnimateWeekChange") as Storyboard;

            FreeEquipmentTypes.EquipmentListBox.SelectionChanged += (o, e) =>
            {
                

                if(FreeEquipmentTypes.EquipmentListBox.Items.Count == 0)
                    AddEquipmentToRoom.IsEnabled = false;

                if (FreeEquipmentTypes.EquipmentListBox.SelectedItem == null)
                {
                    AddEquipmentToRoom.IsEnabled = false;
                }
                else
                {
                    AddEquipmentToRoom.IsEnabled = true;
                    RemoveEquipmentFromRoom.IsEnabled = false;
                    EquipmentTypesInRoom.EquipmentListBox.SelectedItem = null;
                }
            };

            EquipmentTypesInRoom.EquipmentListBox.SelectionChanged += (o, e) =>
            {
                if (EquipmentTypesInRoom.EquipmentListBox.Items.Count == 0)
                    RemoveEquipmentFromRoom.IsEnabled = false;

                if (EquipmentTypesInRoom.EquipmentListBox.SelectedItem == null)
                {
                    RemoveEquipmentFromRoom.IsEnabled = false;
                }
                else
                {
                    AddEquipmentToRoom.IsEnabled = false;
                    RemoveEquipmentFromRoom.IsEnabled = true;
                    FreeEquipmentTypes.EquipmentListBox.SelectedItem = null;
                }
            };

            RoomJoinCheckBox.Checked += (o, e) =>
            {
                RoomSplitCheckbox.IsChecked = false;
                RoomsAvailableToJoinTo.IsEnabled = true;
            };

            RoomJoinCheckBox.Unchecked += (o, e) =>
            {
                RoomsAvailableToJoinTo.IsEnabled = false;
                RoomsAvailableToJoinTo.SelectedItem = null;
            };

            RoomSplitCheckbox.Checked += (o, e) =>
            {
                RoomJoinCheckBox.IsChecked = false;
                RoomsAvailableToJoinTo.IsEnabled = false;
            };

            MakeRoomUnavailableRenovation.Checked += (o, e) =>
            {
                OtherRenovations.IsEnabled = false;
            };

            MakeRoomUnavailableRenovation.Unchecked += (o, e) =>
            {
                OtherRenovations.IsEnabled = true;
            };
        }

        private void RoomList_OnComboBoxSelectionChanged(object sender, MyComboBoxSelectionChangedEventArgs e)
        {
            Storyboard?.Begin();
        }
    }
}