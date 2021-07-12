using GalleryDAL.EF.Interfaces;
using GalleryDAL.EF.Models;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalleryDAL.EF
{
    public class GalleryImageRepository : IRepository<GalleryImage>
    {
        private GalleryContext _galleryContext;

        [Inject]
        public GalleryImageRepository(GalleryContext galleryContext)
        {
            _galleryContext = galleryContext;
        }

        public void AddRange(IEnumerable<GalleryImage> galleryImages)
        {
            foreach (var item in galleryImages)
            {
                _galleryContext.Add(item);
            }
        }

        public void UpdateRange(IEnumerable<GalleryImage> galleryImages)
        {
            foreach (var item in galleryImages)
            {
                var existingItem = GetById(_galleryContext.GalleryImages.FirstOrDefault(img => img.Path == item.Path).ID);
                existingItem.Name = item.Name;
                existingItem.Path = item.Path;
                existingItem.Rating = item.Rating;
                existingItem.Description = item.Description;
            }
            
        }

        public void DeleteRange(IEnumerable<GalleryImage> galleryImages)
        {
            foreach (var image in galleryImages)
            {
                _galleryContext.Remove(image);
            }
        }

        public void SaveChanges()
        {
            _galleryContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _galleryContext.SaveChangesAsync();
        }

        public IEnumerable<GalleryImage> GetAll()
        {
            return _galleryContext.GalleryImages.ToList();
        }

        public GalleryImage GetByPath(string path)
        {
            return _galleryContext.GalleryImages.FirstOrDefault(item => item.Path == path);
        }

        public GalleryImage GetById(int id)
        {
            return _galleryContext.GalleryImages.FirstOrDefault(item => item.ID == id);
        }
    }
}
