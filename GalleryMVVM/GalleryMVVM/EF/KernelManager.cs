using Ninject;
using System;
using System.Collections.Generic;
using System.Text;

namespace GalleryMVVM.EF
{
    public sealed class KernelManager
    {
        private IKernel kernel;

        public IKernel Kernel
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
