using Microsoft.Win32;
using SimpleTaskManager.AppLauncher;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace SimpleTaskManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
