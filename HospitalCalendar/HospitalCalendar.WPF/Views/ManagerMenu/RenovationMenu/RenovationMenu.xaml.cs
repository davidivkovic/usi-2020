﻿using System.Windows.Controls;
using System.Windows.Media.Animation;

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

            AddEquipmentToRoom.IsEnabled = false;
            RemoveEquipmentFromRoom.IsEnabled = false;
            RoomsAvailableToJoinTo.IsEnabled = false;

            Storyboard = RenovationCalendar.FindResource("AnimateWeekChange") as Storyboard;

            FreeEquipmentTypes.EquipmentListBox.SelectionChanged += (o, e) =>
            {


                if (FreeEquipmentTypes.EquipmentListBox.Items.Count == 0)
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
                FreeEquipmentTypes.IsEnabled = false;
                EquipmentTypesInRoom.IsEnabled = false;
                AddEquipmentToRoom.IsEnabled = false;
                RemoveEquipmentFromRoom.IsEnabled = false;
                NewRoomType.IsEnabled = false;
            };

            RoomJoinCheckBox.Unchecked += (o, e) =>
            {
                RoomsAvailableToJoinTo.IsEnabled = false;
                RoomsAvailableToJoinTo.SelectedItem = null;
                FreeEquipmentTypes.IsEnabled = true;
                EquipmentTypesInRoom.IsEnabled = true;
                AddEquipmentToRoom.IsEnabled = true;
                RemoveEquipmentFromRoom.IsEnabled = true;
                NewRoomType.IsEnabled = true;
            };

            RoomSplitCheckbox.Checked += (o, e) =>
            {
                RoomJoinCheckBox.IsChecked = false;
                RoomsAvailableToJoinTo.IsEnabled = false;
                FreeEquipmentTypes.IsEnabled = false;
                EquipmentTypesInRoom.IsEnabled = false;
                AddEquipmentToRoom.IsEnabled = false;
                RemoveEquipmentFromRoom.IsEnabled = false;
                NewRoomType.IsEnabled = false;
            };
            
            RoomSplitCheckbox.Unchecked += (o, e) =>
            {
                FreeEquipmentTypes.IsEnabled = true;
                EquipmentTypesInRoom.IsEnabled = true;
                AddEquipmentToRoom.IsEnabled = true;
                RemoveEquipmentFromRoom.IsEnabled = true;
                NewRoomType.IsEnabled = true;
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
            //Storyboard?.Begin();
        }
    }
}