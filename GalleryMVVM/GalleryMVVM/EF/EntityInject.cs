using GalleryMVVM.EF.Interfaces;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryMVVM.EF
{
    public class EntityInject : NinjectModule
    {
        public override void Load()
        {
            Bind<GalleryContext>().ToSelf().InTransientScope();
            Bind<IGalleryImageRepository>().To<GalleryImageRepository>().InTransientScope();
        }
    }
}
