//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Threading.Tasks.Dataflow;

//namespace KCVDB.LocalAnalyze.IO.Async
//{
//    internal sealed class BlockingIO
//    {
//        private readonly BufferBlock<ArraySegment<byte>> bufferBlock = new BufferBlock<ArraySegment<byte>>();

//        private sealed class OutputStream : Stream
//        {
//            public override bool CanRead
//            {
//                get
//                {
//                    return false;
//                }
//            }

//            public override bool CanSeek
//            {
//                get
//                {
//                    return false;
//                }
//            }

//            public override bool CanWrite
//            {
//                get
//                {
//                    return true;
//                }
//            }

//            public override long Length
//            {
//                get
//                {
//                    throw new NotSupportedException();
//                }
//            }

//            public override long Position
//            {
//                get
//                {
//                    throw new NotSupportedException();
//                }

//                set
//                {
//                    throw new NotSupportedException();
//                }
//            }

//            public override void Flush()
//            {
//            }

//            public override int Read(byte[] buffer, int offset, int count)
//            {
//                throw new NotSupportedException();
//            }

//            public override long Seek(long offset, SeekOrigin origin)
//            {
//                throw new NotSupportedException();
//            }

//            public override void SetLength(long value)
//            {
//                throw new NotSupportedException();
//            }

//            public override async void Write(byte[] buffer, int offset, int count)
//            {
//                await this.io.bufferBlock.SendAsync(new ArraySegment<byte>(buffer, offset, count));
//            }

//            public OutputStream(BlockingIO io)
//            {
//                this.io = io;
//            }

//            private readonly BlockingIO io;
//        }

//        private sealed class InputStream : Stream
//        {
//            public override bool CanRead
//            {
//                get
//                {
//                    return true;
//                }
//            }

//            public override bool CanSeek
//            {
//                get
//                {
//                    return false;
//                }
//            }

//            public override bool CanWrite
//            {
//                get
//                {
//                    return false;
//                }
//            }

//            public override long Length
//            {
//                get
//                {
//                    throw new NotSupportedException();
//                }
//            }

//            public override long Position
//            {
//                get
//                {
//                    throw new NotSupportedException();
//                }

//                set
//                {
//                    throw new NotSupportedException();
//                }
//            }

//            public override void Flush()
//            {
//                throw new NotSupportedException();
//            }

//            public override async int Read(byte[] buffer, int offset, int count)
//            {
//                var target = new Segment { Buffer = buffer, Start = offset, End = offset + count };
//                if (Read(this.source, target))
//                {
//                    foreach (var source in this.io.Segments)
//                    {
//                        this.source = source;
//                        if (!Read(source, target))
//                        {
//                            break;
//                        }
//                    }
//                }
//                return target.Start - offset;
//            }

//            public override long Seek(long offset, SeekOrigin origin)
//            {
//                throw new NotSupportedException();
//            }

//            public override void SetLength(long value)
//            {
//                throw new NotSupportedException();
//            }

//            public override void Write(byte[] buffer, int offset, int count)
//            {
//                throw new NotSupportedException();
//            }

//            public InputStream(BlockingIO io)
//            {
//                this.io = io;
//            }

//            private readonly BlockingIO io;
//        }
//    }
//}
