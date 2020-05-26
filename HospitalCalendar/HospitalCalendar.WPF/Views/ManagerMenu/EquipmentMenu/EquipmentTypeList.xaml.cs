using System;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> viewmodel-development
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

namespace HospitalCalendar.WPF.Views.ManagerMenu.EquipmentMenu
{
    /// <summary>
    /// Interaction logic for EquipmentTypeList.xaml
    /// </summary>
    public partial class EquipmentTypeList : UserControl
    {
        public EquipmentTypeList()
        {
            InitializeComponent();
        }
<<<<<<< HEAD
=======
        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(EquipmentTypeList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
>>>>>>> viewmodel-development
    }
}
