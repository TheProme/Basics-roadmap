using GalleryDAL.EF.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryDAL.EF.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(int id);
        T GetByPath(string path);
        void AddRange(IEnumerable<T> items);
        void UpdateRange(IEnumerable<T> items);
        void DeleteRange(IEnumerable<T> items);
        void SaveChanges();
        Task SaveChangesAsync();
    }
}
