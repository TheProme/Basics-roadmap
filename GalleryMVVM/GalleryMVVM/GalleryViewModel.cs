using GalleryMVVM.EF;
using GalleryMVVM.EF.Interfaces;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GalleryMVVM
{
    public class GalleryViewModel : BaseViewModel
    {
		private IGalleryImageRepository GalleryRep;

		public GalleryViewModel()
		{
			IKernel kernel = new StandardKernel(new EntityInject());
			GalleryRep = kernel.Get<IGalleryImageRepository>();
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

		private GalleryImage _currentImage;

		public GalleryImage CurrentImage
		{
			get { return _currentImage; }
			set { _currentImage = value; OnPropertyChanged(); }
		}

		private ICommand _addImages;
		public ICommand AddImages
		{
			get
			{
				return _addImages ?? 
					(_addImages = new ParametrizedCommand(obj =>
					{
						if(obj != null)
							AddImageToGallery(obj as ObservableCollection<GalleryImage>);
					}));
			}
		}

		private void AddImageToGallery(ObservableCollection<GalleryImage> images)
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
	}
}
