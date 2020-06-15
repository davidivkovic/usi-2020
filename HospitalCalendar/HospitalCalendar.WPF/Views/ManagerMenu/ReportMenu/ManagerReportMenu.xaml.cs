using System;
using HospitalCalendar.WPF.ViewModels.ManagerMenu.ReportMenu;
using MaterialDesignExtensions.Controls;
using System.Windows;
using System.Windows.Controls;

namespace HospitalCalendar.WPF.Views.ManagerMenu.ReportMenu
{
    /// <summary>
    /// Interaction logic for ManagerReportMenu.xaml
    /// </summary>
    public partial class ManagerReportMenu : UserControl
    {
        public ManagerReportMenu()
        {
            InitializeComponent();
        }

        private void FileSystemControl_OnCancel(object sender, RoutedEventArgs e)
        {
            MdDialog.IsOpen = false;
        }

        private void OpenDirectoryControl_OnDirectorySelected(object sender, RoutedEventArgs e)
        {
            var viewModel = (ManagerReportMenuViewModel)DataContext;
            MdDialog.IsOpen = false;
            viewModel.FilePath = ((OpenDirectoryControl)sender).CurrentDirectory;
        }
    }
}