//using System;
//using System.IO;

//namespace KCVDB.LocalAnalyze.IO
//{
//    internal sealed class EventStream : Stream
//    {
//        public override bool CanRead
//        {
//            get
//            {
//                return false;
//            }
//        }

//        public override bool CanSeek
//        {
//            get
//            {
//                return false;
//            }
//        }

//        public override bool CanWrite
//        {
//            get
//            {
//                return true;
//            }
//        }

//        public override long Length
//        {
//            get
//            {
//                throw new NotSupportedException();
//            }
//        }

//        public override long Position
//        {
//            get
//            {
//                throw new NotSupportedException();
//            }

//            set
//            {
//                throw new NotSupportedException();
//            }
//        }

//        protected override void Dispose(bool disposing)
//        {
//            this.reader.Dis
//            base.Dispose(disposing);
//        }

//        public override void Flush()
//        {
//        }

//        public override int Read(byte[] buffer, int offset, int count)
//        {
//            throw new NotSupportedException();
//        }

//        public override long Seek(long offset, SeekOrigin origin)
//        {
//            throw new NotSupportedException();
//        }

//        public override void SetLength(long value)
//        {
//            throw new NotSupportedException();
//        }

//        public override void Write(byte[] buffer, int offset, int count)
//        {
//            this.Writing?.Invoke(new ArraySegment<byte>(buffer, offset, count));
//        }

//        public EventStream(BufferReader reader)
//        {
//            this.reader = reader;
//        }

//        private readonly BufferReader reader;
//    }
//}
