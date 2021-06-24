using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// <summary>
    /// Interaction logic for ApplicationFolderWindow.xaml
    /// </summary>
    public partial class ApplicationFolderWindow : Window
    {
        private ApplicationFolderViewModel ApplicationFolderVM;
        public ApplicationFolderWindow(string path)
        {
            InitializeComponent();
            ApplicationFolderVM = new ApplicationFolderViewModel(path);
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
