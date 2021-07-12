using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GalleryDAL.EF.Interfaces
{

    public interface IUnitOfWork
    {
        void SaveChanges();

        Task SaveChangesAsync();

        void Dispose();
    }
}
