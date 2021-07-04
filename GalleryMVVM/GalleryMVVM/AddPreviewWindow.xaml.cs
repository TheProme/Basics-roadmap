using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace GalleryMVVM
{
    /// <summary>
    /// Interaction logic for AddPreviewWindow.xaml
    /// </summary>
    public partial class AddPreviewWindow : Window
    {
        public ObservableCollection<GalleryImage> PreviewImages { get; private set; }
        public AddPreviewWindow(ObservableCollection<GalleryImage> images)
        {
            InitializeComponent();
            PreviewImages = images;
            this.DataContext = this;
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
