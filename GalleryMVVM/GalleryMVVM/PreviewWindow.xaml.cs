using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GalleryMVVM
{
    /// <summary>
    /// Interaction logic for PreviewWindow.xaml
    /// </summary>
    public partial class PreviewWindow : Window
    {
        public PreviewWindow(ObservableCollection<GalleryImageViewModel> images, GalleryImageViewModel currentImage)
        {
            InitializeComponent();
            this.DataContext = new PreviewWindowViewModel(images, currentImage);
        }
    }
}
