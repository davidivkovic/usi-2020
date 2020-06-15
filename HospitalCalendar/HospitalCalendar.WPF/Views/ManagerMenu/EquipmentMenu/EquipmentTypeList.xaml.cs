using System.Collections;
using System.Windows;
using System.Windows.Controls;

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
        public static DependencyProperty ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable), typeof(EquipmentTypeList),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
    }
}
