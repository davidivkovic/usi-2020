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
using HospitalCalendar.WPF.DataTemplates;

namespace HospitalCalendar.WPF.Views.ManagerMenu.RenovationMenu
{
    /// <summary>
    /// Interaction logic for EquipmentTypeList.xaml
    /// </summary>
    public partial class RenovationEquipmentTypeList : UserControl
    {
        public RenovationEquipmentTypeList()
        {
            InitializeComponent();
        }

        public static DependencyProperty SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(RenovationEquipmentTypeList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
    }
}
