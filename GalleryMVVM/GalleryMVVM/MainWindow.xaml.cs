using GalleryMVVM.EF;
using GalleryMVVM.EF.Interfaces;
using Microsoft.Win32;
using Ninject;
using System.Collections.ObjectModel;
using System.Windows;

namespace GalleryMVVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            IKernel kernel = new StandardKernel(new EntityInject());
            this.DataContext = new GalleryViewModel(kernel.Get<IGalleryImageRepository>());
        }

        private void updateRatingButton_Click(object sender, RoutedEventArgs e)
        {
            popupStatus.IsOpen = true;
        }
    }
}
