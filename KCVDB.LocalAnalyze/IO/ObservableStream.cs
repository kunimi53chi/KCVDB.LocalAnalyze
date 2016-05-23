using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class ObservableStream : Stream, IObservable<ArraySegment<byte>>
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
            if (disposing && !this.isDisposed)
            {
                this.subject.OnCompleted();
                this.subject.Dispose();
                isDisposed = true;
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
            this.subject.OnNext(new ArraySegment<byte>(buffer, offset, count));
        }

        public IDisposable Subscribe(IObserver<ArraySegment<byte>> observer)
        {
            return this.subject.Subscribe(observer);
        }

        private readonly Subject<ArraySegment<byte>> subject = new Subject<ArraySegment<byte>>();

        private bool isDisposed;
    }
}
