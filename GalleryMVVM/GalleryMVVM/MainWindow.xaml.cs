using GalleryDAL.EF;
using GalleryMVVM.MVVM.ViewModels;
using Ninject;
using System.Windows;

namespace GalleryMVVM
{
    /// <summary>
    /// Interaction logic for MainWin.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(new GalleryService(KernelManager.Kernel.Get<UnitOfWork>()));
        }
    }
}
