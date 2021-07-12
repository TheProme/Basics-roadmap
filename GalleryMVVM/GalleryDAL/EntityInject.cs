using GalleryDAL.EF.Interfaces;
using GalleryDAL.EF.Models;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryDAL.EF
{
    public class EntityInject : NinjectModule
    {
        public override void Load()
        {
            Bind<IUnitOfWork>().To<UnitOfWork>().InTransientScope();
        }
    }
}
