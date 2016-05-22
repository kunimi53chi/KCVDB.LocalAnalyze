using System;
using System.Collections.Concurrent;
using System.IO;

namespace KCVDB.LocalAnalyze.IO
{
    public sealed class BlockingIO : IDisposable
    {
        public BlockingIO()
        {
            this.Input = new InputStream(this);
            this.Output = new OutputStream(this);
        }

        public BlockingIO(long length)
            : this()
        {
            this.length = length;
        }

        public void CompleteAdding()
        {
            this.Segments.CompleteAdding();
        }

        public void Dispose()
        {
            if (!this.isDisposed)
            {
                this.segments?.Dispose();
                this.isDisposed = true;
            }
        }

        public event Action StartingInput;

        private BlockingCollection<Segment> Segments
        {
            get
            {
                if (this.isDisposed)
                {
                    throw new ObjectDisposedException(typeof(BlockingIO).FullName);
                }
                else
                {
                    if (this.segments == null)
                    {
                        this.segments = new BlockingCollection<Segment>(1);
                    }
                    return this.segments;
                }
            }
        }

        private BlockingCollection<Segment> segments;
        private bool isDisposed;
        private readonly long? length;
        public readonly Stream Input;
        public readonly Stream Output;

        private sealed class InputStream : Stream
        {
            public override bool CanRead
            {
                get
                {
                    return true;
                }
            }

            public override bool CanSeek
            {
                get
                {
                    return this.io.length != null;
                }
            }

            public override bool CanWrite
            {
                get
                {
                    return false;
                }
            }

            public override long Length
            {
                get
                {
                    if (this.io.length != null)
                    {
                        return this.io.length.Value;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
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
                if (disposing)
                {
                    this.io.Dispose();
                }
                base.Dispose(disposing);
            }

            public override void Flush()
            {
                throw new NotSupportedException();
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (!this.isStartingInput)
                {
                    this.isStartingInput = true;
                    this.io.StartingInput?.Invoke();
                }
                var target = new Segment { Buffer = buffer, Start = offset, End = offset + count };
                if (Read(this.source, target))
                {
                    foreach (var source in this.io.Segments)
                    {
                        this.source = source;
                        if (!Read(source, target))
                        {
                            break;
                        }
                    }
                }
                return target.Start - offset;
            }

            private static bool Read(Segment source, Segment target)
            {
                var sourceCount = source.End - source.Start;
                var targetCount = target.End - target.Start;
                int count;
                bool repeats;
                if (sourceCount < targetCount)
                {
                    count = sourceCount;
                    repeats = true;
                }
                else
                {
                    count = targetCount;
                    repeats = false;
                }
                Array.Copy(source.Buffer, source.Start, target.Buffer, target.Start, count);
                source.Start += count;
                target.Start += count;
                return repeats;
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
                throw new NotSupportedException();
            }

            public InputStream(BlockingIO io)
            {
                this.io = io;
            }

            private readonly BlockingIO io;
            private Segment source = new Segment { };
            private bool isStartingInput;
        }

        private sealed class OutputStream : Stream
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
                    return this.io.length != null;
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
                    if (this.io.length != null)
                    {
                        return this.io.length.Value;
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
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
                if (disposing)
                {
                    this.io.CompleteAdding();
                }
                base.Dispose(disposing);
            }

            public override void Flush()
            {
                throw new NotSupportedException();
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
                this.io.Segments.Add(new Segment { Buffer = buffer, Start = offset, End = offset + count });
            }

            public OutputStream(BlockingIO io)
            {
                this.io = io;
            }

            private readonly BlockingIO io;
        }

        private sealed class Segment
        {
            public byte[] Buffer;
            public int Start;
            public int End;
        }
    }
}
