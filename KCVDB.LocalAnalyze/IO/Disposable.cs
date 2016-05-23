using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class Disposable : IDisposable
    {
        public void Dispose()
        {
            throw new ObjectDisposedException(this.GetType().FullName);
        }
    }
}
