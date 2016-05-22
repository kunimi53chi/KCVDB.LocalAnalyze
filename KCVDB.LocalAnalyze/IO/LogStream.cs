using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    struct LogStream
    {
        public readonly string Name;
        public readonly Stream Stream;

        public LogStream(string name, Stream stream)
        {
            this.Name = name;
            this.Stream = stream;
        }
    }
}
