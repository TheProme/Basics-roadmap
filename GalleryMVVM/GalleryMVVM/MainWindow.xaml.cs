using Microsoft.Win32;
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
            this.DataContext = new GalleryViewModel();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog() == true)
            {
                ObservableCollection<GalleryImage> previewImages = new ObservableCollection<GalleryImage>();
                foreach (var file in openFileDialog.FileNames)
                {
                    previewImages.Add(new GalleryImage { Path = file, Name = "untitled", IsChangable = true });
                }
                AddPreviewWindow previewWindow = new AddPreviewWindow(previewImages);
                if (previewWindow.ShowDialog() == true)
                {
                    addButton.CommandParameter = previewWindow.PreviewImages;
                }
            }
        }
    }
}
