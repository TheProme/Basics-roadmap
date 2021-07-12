using GalleryDAL.EF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMVVM.MVVM.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public FolderViewModel FolderViewModel { get; set; }
        public GalleryViewModel GalleryViewModel { get; set; }

        public MainViewModel(GalleryService service)
        {
            GalleryViewModel = new GalleryViewModel(service);
            FolderViewModel = new FolderViewModel();
            FolderViewModel.FolderChanged += FolderChangedHandler;
        }

        private void FolderChangedHandler(List<string> imagePaths)
        {
            GalleryViewModel.SetImagesFromList(imagePaths);
        }
    }
}
