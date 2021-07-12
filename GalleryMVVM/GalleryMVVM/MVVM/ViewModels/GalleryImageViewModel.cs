using GalleryDAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryMVVM
{
    public class GalleryImageViewModel : BaseViewModel
    {
		public delegate void DataChanged();
		public event DataChanged DataChangedEvent;

		public delegate void DeleteImage(GalleryImageViewModel imageToDelete);
		public event DeleteImage DeleteImageEvent;

		private GalleryImage GalleryImage;
		public GalleryImageViewModel(GalleryImage imageModel)
		{
			GalleryImage = imageModel;
		}
		public int ID
		{
			get =>  GalleryImage.ID;
			set 
			{ 
				GalleryImage.ID = value;
				OnPropertyChanged();
			}
		}
		public string Name
		{
			get => GalleryImage.Name ?? System.IO.Path.GetFileName(GalleryImage.Path);
			set
			{
				GalleryImage.Name = value;
				//TODO: Update FileName
				DataChangedEvent?.Invoke();
				OnPropertyChanged();
			}
		}
		public string Description
		{
			get => GalleryImage.Description;
			set
			{
				GalleryImage.Description = value;
				DataChangedEvent?.Invoke();
				OnPropertyChanged();
			}
		}
		public int Rating
		{
			get => GalleryImage.Rating;
			set
			{
				GalleryImage.Rating = value;
				DataChangedEvent?.Invoke();
				OnPropertyChanged();
			}
		}
		public string Path
		{
			get => GalleryImage.Path;
			set
			{
				GalleryImage.Path = value;
				DataChangedEvent?.Invoke();
				OnPropertyChanged();
			}
		}

		private ICommand _deleteImageCommand;

		public ICommand DeleteImageCommand
		{
			get => _deleteImageCommand ?? (_deleteImageCommand = new ParametrizedCommand(obj =>
			{
				DeleteImageEvent?.Invoke(this);
			}));
		}

		private ICommand _viewFullSize;

		public ICommand ViewFullSize
		{
			get => _viewFullSize ?? (_viewFullSize = new ParametrizedCommand(obj =>
			{
				new PreviewWindow(this).Show();
			}));
		}

		private bool _isFavorite;

		public bool IsFavorite
		{
			get => _isFavorite; 
			set 
			{ 
				_isFavorite = value;
				OnPropertyChanged();
			}
		}


		private bool _isChangable;

		public bool IsChangable
		{
			get => _isChangable; 
			set 
			{ 
				_isChangable = value; 
				OnPropertyChanged(); 
			}
		}
	}
}
