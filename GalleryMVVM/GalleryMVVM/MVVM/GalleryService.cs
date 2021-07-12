using GalleryDAL.EF;
using GalleryDAL.EF.Interfaces;
using GalleryDAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMVVM
{
    public class GalleryService: IGalleryService
    {
        private UnitOfWork _galleryUOW;

        private List<GalleryImage> ImagesToAdd { get; set; } = new List<GalleryImage>();
        private List<GalleryImage> ImagesToUpdate { get; set; } = new List<GalleryImage>();
        private List<GalleryImage> ImagesToDelete { get; set; } = new List<GalleryImage>();

        public GalleryService(UnitOfWork galleryUOW)
        {
            _galleryUOW = galleryUOW;
        }


        public List<GalleryImageViewModel> GetAll()
        {
            return CastModelsToVM(_galleryUOW.GalleryImages.GetAll());
        }
        private List<GalleryImageViewModel> CastModelsToVM(IEnumerable<GalleryImage> imageModels)
        {
            if(imageModels != null && imageModels.Count() > 0)
            {
                List<GalleryImageViewModel> viewModels = new List<GalleryImageViewModel>();
                foreach (var item in imageModels)
                {
                    viewModels.Add(CastModelToVM(item));
                }
                return viewModels;
            }
            return null;
        }

        private GalleryImageViewModel CastModelToVM(GalleryImage imageModel)
        {
            return new GalleryImageViewModel(imageModel);
        }

        private GalleryImage CastVMToModel(GalleryImageViewModel imageVM)
        {
            return _galleryUOW.GalleryImages.GetByPath(imageVM.Path) ?? new GalleryImage { ID = imageVM.ID, Name = imageVM.Name, Description = imageVM.Description, Path = imageVM.Path, Rating = imageVM.Rating };
        }


        private List<GalleryImage> CastVMToModels(IEnumerable<GalleryImageViewModel> imageVMs)
        {
            if (imageVMs != null && imageVMs.Count() > 0)
            {
                List<GalleryImage> models = new List<GalleryImage>();
                foreach (var item in imageVMs)
                {
                    models.Add(CastVMToModel(item));
                }
                return models;
            }
            return null;
        }

        public GalleryImageViewModel GetImageByPath(string path)
        {
            var existingItem = _galleryUOW.GalleryImages.GetByPath(path) ?? ImagesToAdd.FirstOrDefault(img=>img.Path == path) ?? ImagesToUpdate.FirstOrDefault(img => img.Path == path) ?? ImagesToDelete.FirstOrDefault(img => img.Path == path);
            if(existingItem != null)
            {
                return new GalleryImageViewModel(existingItem);
            }
            return null;
        }

        public void AddData(List<GalleryImageViewModel> galleryImages)
        {
            foreach (var image in CastVMToModels(galleryImages))
            {
                if (image.ID == 0 && !_galleryUOW.GalleryImages.GetAll().Any(item => item.Path == image.Path) && !ImagesToAdd.Contains(image))
                {
                    ImagesToAdd.Add(image);
                }
            }
        }
        public void AddData(GalleryImageViewModel galleryImage)
        {
            var image = CastVMToModel(galleryImage);
            if (image.ID == 0 && !_galleryUOW.GalleryImages.GetAll().Any(item => item.Path == image.Path) && !ImagesToAdd.Contains(image))
            {
                ImagesToAdd.Add(image);
            }
        }

        public void UpdateData(List<GalleryImageViewModel> galleryImages)
        {
            foreach (var image in CastVMToModels(galleryImages))
            {
                if (image.ID != 0 && !ImagesToAdd.Contains(image) && !ImagesToUpdate.Contains(image))
                {
                    ImagesToUpdate.Add(image);
                }
            }
        }

        public void UpdateData(GalleryImageViewModel galleryImage)
        {
            var image = CastVMToModel(galleryImage);
            if (image.ID != 0 && !ImagesToAdd.Contains(image) && !ImagesToUpdate.Contains(image))
            {
                image.Name = galleryImage.Name;
                image.Path = galleryImage.Path;
                image.Description = galleryImage.Description;
                image.Rating = galleryImage.Rating;
                ImagesToUpdate.Add(image);
            }
            else
            {
                var existingImage = ImagesToUpdate.FirstOrDefault(img => img.ID == galleryImage.ID) ?? ImagesToAdd.FirstOrDefault(img => img.ID == galleryImage.ID);
                if(existingImage != null)
                {
                    existingImage.Name = galleryImage.Name;
                    existingImage.Path = galleryImage.Path;
                    existingImage.Rating = galleryImage.Rating;
                    existingImage.Description = galleryImage.Description;
                }
            }
        }

        public void DeleteData(List<GalleryImageViewModel> galleryImages)
        {
            foreach (var image in CastVMToModels(galleryImages))
            {
                ImagesToAdd.Remove(image);
                ImagesToUpdate.Remove(image);
                if (!ImagesToDelete.Contains(image))
                {
                    ImagesToDelete.Add(image);
                }
            }
        }
        public void DeleteData(GalleryImageViewModel galleryImage)
        {
            var image = CastVMToModel(galleryImage);
            ImagesToAdd.Remove(image);
            ImagesToUpdate.Remove(image);
            if (!ImagesToDelete.Contains(image))
            {
                ImagesToDelete.Add(image);
            }
        }

        public void SaveChanges()
        {
            if(ImagesToAdd.Count > 0 || ImagesToDelete.Count > 0 || ImagesToUpdate.Count > 0)
            {
                _galleryUOW.GalleryImages.AddRange(ImagesToAdd);
                _galleryUOW.GalleryImages.UpdateRange(ImagesToUpdate);
                _galleryUOW.GalleryImages.DeleteRange(ImagesToDelete);
                _galleryUOW.SaveChanges();
                ImagesToAdd.Clear();
                ImagesToUpdate.Clear();
                ImagesToDelete.Clear();
            }
        }

        public async Task SaveChangesAsync()
        {
            _galleryUOW.GalleryImages.AddRange(ImagesToAdd);
            _galleryUOW.GalleryImages.UpdateRange(ImagesToUpdate);
            _galleryUOW.GalleryImages.DeleteRange(ImagesToDelete);
            await _galleryUOW.SaveChangesAsync();
            ImagesToAdd.Clear();
            ImagesToUpdate.Clear();
            ImagesToDelete.Clear();
        }
    }
}
