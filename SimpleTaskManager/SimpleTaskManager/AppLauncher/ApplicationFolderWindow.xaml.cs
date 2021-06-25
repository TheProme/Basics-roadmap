using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SimpleTaskManager.AppLauncher
{
    public class EmptyListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value) > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Interaction logic for ApplicationFolderWindow.xaml
    /// </summary>
    public partial class ApplicationFolderWindow : Window
    {
        private ApplicationFolderViewModel ApplicationFolderVM;
        public ApplicationFolderWindow(string installPath)
        {
            InitializeComponent();
            ApplicationFolderVM = new ApplicationFolderViewModel(installPath);
            DataContext = ApplicationFolderVM;
        }

        public string SelectedFilePath { get { return ApplicationFolderVM.SelectedExecutable.FullPath; } }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
