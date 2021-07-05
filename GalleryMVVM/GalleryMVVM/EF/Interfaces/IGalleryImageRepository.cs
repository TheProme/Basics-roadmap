﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryMVVM.EF.Interfaces
{
    public interface IGalleryImageRepository
    {
        List<GalleryImage> GetAll();
        GalleryImage GetById(int id);
        void Add(List<GalleryImage> galleryImages);
        void Delete(List<GalleryImage> galleryImages);
    }
}