using GalleryDAL.EF.Models;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryMVVM
{
	public class GalleryViewModel : BaseViewModel
    {
		private GalleryService DALService;
		private PreviewWindow _previewWindow;
		public GalleryViewModel(GalleryService dalService)
		{
			DALService = dalService;
			ReadImagesFromRepository();
		}

		private void ReadImagesFromRepository()
		{
			ClearImages();
			foreach (var item in DALService.GetAll())
			{
				item.DeleteImageEvent += DeleteImageHandler;
				item.FullSizeClickEvent += FullSizeClickHandler;
				item.IsFavorite = true;
				GalleryImages.Add(item);
			}
		}

		private void FullSizeClickHandler(GalleryImageViewModel currentImage)
		{
			if(_previewWindow != null)
			{
				_previewWindow.Close();
			}
			_previewWindow = new PreviewWindow(GalleryImages, currentImage);
			_previewWindow.Show();
		}

		private void DeleteImageHandler(GalleryImageViewModel imageToDelete)
		{
			RemoveFromCollection(imageToDelete);
			DALService.DeleteData(imageToDelete);
			DALService.SaveChanges();
		}

		public void SetImagesFromList(List<string> imagePaths)
		{
			ClearImages();
			foreach (var item in imagePaths)
			{
				var existingImage = DALService.GetImageByPath(item);
				if (existingImage != null)
				{
					existingImage.DeleteImageEvent += DeleteImageHandler;
					existingImage.FullSizeClickEvent += FullSizeClickHandler;
					existingImage.IsFavorite = true;
					GalleryImages.Add(existingImage);
				}
				else
				{
					GalleryImageViewModel newImage = new GalleryImageViewModel(new GalleryImage { Path = item });
					newImage.FullSizeClickEvent += FullSizeClickHandler;
					GalleryImages.Add(newImage);
				}
			}
		}


		private void RemoveFromCollection(GalleryImageViewModel imageToDelete)
		{
			imageToDelete.DeleteImageEvent -= DeleteImageHandler;
			imageToDelete.DataChangedEvent -= ImageDataChangedHandler;
			imageToDelete.FullSizeClickEvent -= FullSizeClickHandler;
			if (GalleryImages.All(img=> img.IsFavorite))
			{
				GalleryImages.Remove(imageToDelete);
			}
			else
			{
				var index = GalleryImages.IndexOf(imageToDelete);
				GalleryImages.Remove(imageToDelete);
				GalleryImages.Insert(index, new GalleryImageViewModel(new GalleryImage { Path = imageToDelete.Path }));
			}
			
		}

		private void ClearImages()
		{
			foreach (var image in GalleryImages)
			{
				if(image.IsFavorite)
				{
					image.DeleteImageEvent -= DeleteImageHandler;
				}
				image.DataChangedEvent -= ImageDataChangedHandler;
				image.FullSizeClickEvent -= FullSizeClickHandler;
			}
			GalleryImages.Clear();
		}

		public ObservableCollection<GalleryImageViewModel> GalleryImages { get; set; } = new ObservableCollection<GalleryImageViewModel>();

		private GalleryImageViewModel _currentImage;

		public GalleryImageViewModel CurrentImage
		{
			get => _currentImage; 
			set 
			{ 
				if(_currentImage != null)
				{
					_currentImage.DataChangedEvent -= ImageDataChangedHandler;
				}
				_currentImage = value;
				if (_currentImage != null)
				{
					_currentImage.DataChangedEvent += ImageDataChangedHandler;
				}
				OnPropertyChanged(); 
			}
		}

		private void ImageDataChangedHandler()
		{
			if(DALService.GetImageByPath(CurrentImage.Path) != null)
			{
				DALService.UpdateData(CurrentImage);
			}
			else
			{
				DALService.AddData(CurrentImage);
			}
			CurrentImage.IsFavorite = true;
			DALService.SaveChanges();
		}

		private ICommand _showFavorites;
		public ICommand ShowFavorites
		{
			get
			{
				return _showFavorites ?? (_showFavorites = new ParametrizedCommand(obj =>
				{
					ReadImagesFromRepository();
				}));
			}
		}
	}
}
