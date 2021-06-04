using System;
using System.Threading.Tasks;

namespace ThreadPoolSync
{
    public interface ISynchronisationContext
    {
        bool Execute(Func<bool> action);
        void AbortThread();
    }
}