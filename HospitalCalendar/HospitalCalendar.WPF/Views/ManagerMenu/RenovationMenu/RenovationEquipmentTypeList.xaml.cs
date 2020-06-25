using System.Collections;
using System.Windows;
using System.Windows.Controls;

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

        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(RenovationEquipmentTypeList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
    }
}
