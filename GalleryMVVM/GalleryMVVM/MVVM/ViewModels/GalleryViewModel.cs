using GalleryMVVM.EF.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace GalleryMVVM
{
	public class GalleryViewModel : BaseViewModel
    {
		private IGalleryImageRepository GalleryRep;

		public GalleryViewModel(IGalleryImageRepository galleryRepository)
		{
			GalleryRep = galleryRepository;
			ReadImagesFromRepository(GalleryRep);
		}

		private void ReadImagesFromRepository(IGalleryImageRepository repository)
		{
			foreach (var item in repository.GetAll())
			{
				GalleryImages.Add(item);
			}
		}

		public ObservableCollection<GalleryImage> GalleryImages { get; set; } = new ObservableCollection<GalleryImage>();

		private ObservableCollection<GalleryImage> _currentImages = new ObservableCollection<GalleryImage>();

		public ObservableCollection<GalleryImage> CurrentImages
		{
			get { return _currentImages; }
			set { _currentImages = value; OnPropertyChanged(); }
		}

		private ICommand _addImages;
		public ICommand AddImages
		{
			get
			{
				return _addImages ?? 
					(_addImages = new ParametrizedCommand(obj =>
					{
						OpenPreviewWindow();
					}));
			}
		}

		private ICommand _deleteImages;
		public ICommand DeleteImages
		{
			get
			{
				return _deleteImages ??
					(_deleteImages = new ParametrizedCommand(obj =>
					{
						if (obj != null)
							DeleteSelectedImages((obj as ObservableCollection<GalleryImage>).ToList());
					}, obj => obj != null && (obj as ObservableCollection<GalleryImage>).Count > 0));
			}
		}

		private ICommand _editImages;
		public ICommand EditImages
		{
			get
			{
				return _editImages ??
					(_editImages = new ParametrizedCommand(obj =>
					{
						if (obj != null)
							EditSelectedImages((obj as ObservableCollection<GalleryImage>).ToList());
					}, obj => obj != null && (obj as ObservableCollection<GalleryImage>).Count > 0));
			}
		}

		private ICommand _saveRating;
		public ICommand SaveRating
		{
			get
			{
				return _saveRating ??
					(_saveRating = new ParametrizedCommand(obj =>
					{
						SaveImagesRating();
					}, obj => obj != null && (obj as ObservableCollection<GalleryImage>).Count > 0));
			}
		}

		private void OpenPreviewWindow(List<GalleryImage> previewImages = null)
		{
			if(previewImages == null)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";
				openFileDialog.Multiselect = true;
				if (openFileDialog.ShowDialog() == true)
				{
					previewImages = new List<GalleryImage>();
					foreach (var file in openFileDialog.FileNames)
					{
						previewImages.Add(new GalleryImage { Path = file, Name = "untitled"});
					}
					EditImageWindow previewWindow = new EditImageWindow(previewImages);
					if (previewWindow.ShowDialog() == true)
					{
						AddImagesToGallery(previewWindow.PreviewImages.ToList());
					}
				}
			}
			else
			{
				EditImageWindow previewWindow = new EditImageWindow(previewImages);
				if (previewWindow.ShowDialog() == true)
				{
					AddImagesToGallery(previewWindow.PreviewImages.ToList());
				}
			}
		}

		private void AddImagesToGallery(List<GalleryImage> images)
		{
			foreach (var image in images)
			{
				image.IsChangable = false;
				if (!GalleryImages.Any(img => img.Path == image.Path))
				{
					GalleryImages.Add(image);
				}
				else
				{
					var existingItem = GalleryImages.FirstOrDefault(img => img.Path == image.Path);
					var index = GalleryImages.IndexOf(existingItem);
					GalleryImages.Remove(existingItem);
					GalleryImages.Insert(index, image);
				}
			}
			GalleryRep.Add(images.ToList());
		}

		private void DeleteSelectedImages(List<GalleryImage> images)
		{
			foreach (var item in images)
			{
				GalleryImages.Remove(item);
			}
			GalleryRep.Delete(images);
		}

		private void EditSelectedImages(List<GalleryImage> images)
		{
			OpenPreviewWindow(images);
		}

		private void SaveImagesRating()
		{
			GalleryRep.Add(GalleryImages.ToList());
			CurrentImages.Clear();
		}
	}
}
