using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class ObservableTextReader
    {
        public IObservable<string> ReadLine(IObservable<ArraySegment<byte>> source)
        {
            var subject = new Subject<IEnumerable<string>>();
            source.Subscribe(
                segment =>
                {
                    subject.OnNext(this.ReadLines(segment));
                },
                error =>
                {
                    subject.OnError(error);
                },
                () =>
                {
                    subject.OnNext(this.ReadLastLines());
                    subject.OnCompleted();
                });
            return subject.SelectMany(lines => lines.ToObservable());
        }

        private IEnumerable<string> ReadLines(ArraySegment<byte> source)
        {
            var offset = source.Offset;
            var end = source.Offset + source.Count;
            for (var i = offset; i < end; ++i)
            {
                if (source.Array[i] == '\r' || (previousByte != '\r' && source.Array[i] == '\n'))
                {
                    this.builder.Append(source.Array, offset, i - offset);
                    yield return Encoding.UTF8.GetString(this.builder.Array, 0, this.builder.Count);
                    this.builder.Clear();
                }
                if (source.Array[i] == '\r' || source.Array[i] == '\n')
                {
                    offset = i + 1;
                }
                this.previousByte = source.Array[i];
            }
            this.builder.Append(source.Array, offset, end - offset);
        }

        private IEnumerable<string> ReadLastLines()
        {
            if (this.builder.Count > 0)
            {
                yield return Encoding.UTF8.GetString(this.builder.Array, 0, this.builder.Count);
                this.builder.Clear();
            }
        }

        private readonly ArrayBuilder<byte> builder = new ArrayBuilder<byte>();

        private int previousByte = -1;
    }
}
