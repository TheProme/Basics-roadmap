using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace GalleryMVVM
{
    public class PreviewWindowViewModel: BaseViewModel
    {
        public ObservableCollection<GalleryImageViewModel> Images { get; set; }
        private GalleryImageViewModel _currentImage;
        public GalleryImageViewModel CurrentImage 
        {
            get => _currentImage;
            set
            {
                _currentImage = value;
                _index = value != null ? Images.IndexOf(_currentImage) : _index;
                OnPropertyChanged();
            }
        }
        private int _index;
        public PreviewWindowViewModel(ObservableCollection<GalleryImageViewModel> images, GalleryImageViewModel currentImage)
        {
            Images = images;
            _index = Images.IndexOf(currentImage);
            CurrentImage = currentImage;
        }

        private ICommand _nextImage;
        public ICommand NextImage
        {
            get => _nextImage ?? (_nextImage = new RelayCommand(obj =>
            {
                _index = _index < Images.Count - 1 ? ++_index : 0;
                CurrentImage = Images[_index];
            }));
        }

        private ICommand _previousImage;
        public ICommand PreviousImage
        {
            get => _previousImage ?? (_previousImage = new RelayCommand(obj =>
            {
                _index = _index > 0 ? --_index : Images.Count - 1;
                CurrentImage = Images[_index];
            }));
        }
    }
}
