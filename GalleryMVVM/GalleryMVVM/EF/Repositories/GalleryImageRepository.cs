using GalleryMVVM.EF.Interfaces;
using Ninject;
using System.Collections.Generic;
using System.Linq;

namespace GalleryMVVM.EF
{
    public class GalleryImageRepository : IGalleryImageRepository
    {
        private GalleryContext _galleryContext;

        [Inject]
        public GalleryImageRepository(GalleryContext galleryContext)
        {
            _galleryContext = galleryContext;
        }

        public void Add(List<GalleryImage> galleryImages)
        {
            foreach (var item in galleryImages)
            {
                if (!_galleryContext.GalleryImages.Any(img => img.Path == item.Path))
                {
                    _galleryContext.Add(item);
                }
                else
                {
                    var existingItem = GetById(_galleryContext.GalleryImages.FirstOrDefault(img=> img.Path == item.Path).ID);
                    existingItem.Name = item.Name;
                    existingItem.Path = item.Path;
                    existingItem.Rating = item.Rating;
                    existingItem.Description = item.Description;
                }
            }
            _galleryContext.SaveChanges();
        }

        public void Delete(List<GalleryImage> galleryImages)
        {
            foreach (var image in galleryImages)
            {
                _galleryContext.Remove(_galleryContext.GalleryImages.FirstOrDefault(item => item.Path == image.Path));
            }
            _galleryContext.SaveChanges();
        }

        public List<GalleryImage> GetAll()
        {
            return _galleryContext.GalleryImages.ToList();
        }

        public GalleryImage GetById(int id)
        {
            return _galleryContext.GalleryImages.FirstOrDefault(item => item.ID == id);
        }
    }
}
