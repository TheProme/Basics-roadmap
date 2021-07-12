using GalleryDAL.EF.Interfaces;
using GalleryDAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryMVVM
{
    public interface IGalleryService
    {
        List<GalleryImageViewModel> GetAll();
        void AddData(List<GalleryImageViewModel> items);
        void AddData(GalleryImageViewModel item);
        void DeleteData(List<GalleryImageViewModel> items);
        void DeleteData(GalleryImageViewModel item);
        void UpdateData(List<GalleryImageViewModel> items);
        void UpdateData(GalleryImageViewModel item);

        GalleryImageViewModel GetImageByPath(string path);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
