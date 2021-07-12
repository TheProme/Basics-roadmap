using GalleryDAL.EF.Interfaces;
using GalleryDAL.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryDAL.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        public GalleryContext GalleryDbContext { get; }

        public IRepository<GalleryImage> GalleryImages { get; }

        public UnitOfWork(GalleryContext context)
        {
            GalleryDbContext = context ?? throw new ArgumentNullException(nameof(context));
            GalleryImages = new GalleryImageRepository(GalleryDbContext as GalleryContext);
        }

        public void SaveChanges()
        {
            GalleryImages.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await GalleryImages.SaveChangesAsync();
        }

        public void Dispose()
        {
            GalleryDbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
