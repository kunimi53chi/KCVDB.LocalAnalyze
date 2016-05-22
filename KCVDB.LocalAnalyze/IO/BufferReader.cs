using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    sealed class BufferReader
    {
        public IEnumerable<string> ReadLastLines()
        {
            yield return Encoding.UTF8.GetString(this.segment.Array, this.segment.Offset, this.segment.Count);
        }

        public IEnumerable<string> ReadLines(byte[] buffer, int offset, int count)
        {
            var end = offset + count;
            int i;
            for (i = offset; i < end; )
            {
                if (buffer[i] == '\r' || buffer[i] == '\n')
                {
                    if (this.segment.Count > 0)
                    {
                        var bytes = new byte[this.segment.Count + (i - offset)];
                        Array.Copy(bytes, 0, this.segment.Array, this.segment.Offset, this.segment.Count);
                        Array.Copy(bytes, this.segment.Count, buffer, offset, i - offset);
                        yield return Encoding.UTF8.GetString(bytes);
                        this.segment = new ArraySegment<byte> { };
                    }
                    else
                    {
                        yield return Encoding.UTF8.GetString(buffer, offset, i - offset);
                    }
                    if (buffer[i] == '\r' && (i + 1) < count && buffer[i + 1] == '\n')
                    {
                        i += 2;
                    }
                    else
                    {
                        ++i;
                    }
                    offset = i;
                }
                else
                {
                    ++i;
                }
            }
            if (end - offset > 0)
            {
                this.segment = new ArraySegment<byte>(buffer, offset, end - offset);
            }
            else
            {
                this.segment = new ArraySegment<byte>();
            }
        }

        private ArraySegment<byte> segment;
    }
}
