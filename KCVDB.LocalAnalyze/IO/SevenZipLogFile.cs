using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class SevenZipLogFile : LogFile
    {
        public SevenZipLogFile(Log log, DateTimeOffset date, Guid sessionId)
            : base(log, date, sessionId)
        {
        }

        public void StartReading(ExtractFileCallbackArgs args)
        {
            args.ExtractToStream = new SevenZipStream(this);
        }

        private sealed class SevenZipStream : Stream
        {
            public override bool CanRead
            {
                get
                {
                    return false;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return false;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return true;
                }
            }

            public override long Length
            {
                get
                {
                    throw new NotSupportedException();
                }
            }

            public override long Position
            {
                get
                {
                    throw new NotSupportedException();
                }

                set
                {
                    throw new NotSupportedException();
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (!this.isDisposed)
                {
                    this.logFile.readingBuffer.OnCompleted();
                    this.isDisposed = true;
                }
                base.Dispose(disposing);
            }

            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                this.logFile.readingBuffer.OnNext(new ArraySegment<byte>(buffer, offset, count));
            }

            public SevenZipStream(SevenZipLogFile logFile)
            {
                this.logFile = logFile;
            }

            private readonly SevenZipLogFile logFile;
            private bool isDisposed;
        }
    }
}
