using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace SimpleTaskManager.AppLauncher
{
    /// <summary>
    /// Interaction logic for ApplicationFolderWindow.xaml
    /// </summary>
    public partial class ApplicationFolderWindow : Window
    {
        public ApplicationFolderWindow(string path)
        {
            DataContext = this;
            InitializeComponent();
            if(!String.IsNullOrEmpty(path))
            {
                SetExecutablesFromPath(path);
            }
        }

        public string FilePath
        {
            get { return (string)GetValue(FilePathProperty); }
            set { SetValue(FilePathProperty, value); }
        }

        public static readonly DependencyProperty FilePathProperty =
            DependencyProperty.Register("FilePath", typeof(string), typeof(ApplicationFolderWindow), new PropertyMetadata(null));



        private void SetExecutablesFromPath(string path)
        {
            filesList.ItemsSource = Directory.GetFiles(path, "*.EXE", SearchOption.AllDirectories).ToList();
        }

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
