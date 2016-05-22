using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    sealed class StreamLogFile : LogFile
    {
        public StreamLogFile(Log manager, DateTimeOffset date, Guid sessionId)
            : base(manager, date, sessionId)
        {
        }

        public void StartReading(Stream stream)
        {
            var buffer = new byte[1 << 21];
            int count;
            while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                this.readingBuffer.OnNext(new ArraySegment<byte>(buffer, 0, count));
            }
            this.readingBuffer.OnCompleted();
        }
    }
}
