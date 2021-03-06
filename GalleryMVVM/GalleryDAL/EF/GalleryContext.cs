using GalleryDAL.EF.Configurations;
using GalleryDAL.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace GalleryDAL.EF
{
    public class GalleryContext: DbContext
    {
        public GalleryContext()
        {
            Database.EnsureCreated();
        }

        public DbSet<GalleryImage> GalleryImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=GalleryDB.db");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new GalleryImageConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
