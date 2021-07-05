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
    /// Interaction logic for EditImageWindow.xaml
    /// </summary>
    public partial class EditImageWindow : Window
    {
        public ObservableCollection<GalleryImage> PreviewImages { get; private set; } = new ObservableCollection<GalleryImage>();

        public EditImageWindow(List<GalleryImage> images)
        {
            InitializeComponent();
            foreach (var item in images)
            {
                item.IsChangable = true;
                PreviewImages.Add(item);
            }
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
