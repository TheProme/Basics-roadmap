using Ninject;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryDAL.EF
{
    public sealed class KernelManager
    {
        private static IKernel kernel;

        public static IKernel Kernel
        {
            get
            {
                if (kernel == null)
                {
                    kernel = new StandardKernel(new EntityInject());
                }
                return kernel;
            }
        }
    }
}
