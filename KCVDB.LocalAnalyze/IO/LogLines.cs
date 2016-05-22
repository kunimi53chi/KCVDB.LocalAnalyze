using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    public struct LogLines
    {
        public readonly string Name;
        public readonly IEnumerable<string> Lines;

        public LogLines(string name, IEnumerable<string> lines)
        {
            this.Name = name;
            this.Lines = lines;
        }
    }
}
