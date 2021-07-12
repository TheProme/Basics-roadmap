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
				item.IsFavorite = true;
				GalleryImages.Add(item);
			}
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
					existingImage.IsFavorite = true;
					GalleryImages.Add(existingImage);
				}
				else
				{
					GalleryImages.Add(new GalleryImageViewModel(new GalleryImage { Path = item }));
				}
			}
		}


		private void RemoveFromCollection(GalleryImageViewModel imageToDelete)
		{
			imageToDelete.DeleteImageEvent -= DeleteImageHandler;
			imageToDelete.DataChangedEvent -= ImageDataChangedHandler;
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
