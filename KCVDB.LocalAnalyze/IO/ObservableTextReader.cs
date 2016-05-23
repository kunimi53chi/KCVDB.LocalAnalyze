using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class ObservableTextReader : IObserver<ArraySegment<byte>>, IObservable<string>
    {
        public IDisposable Subscribe(IObserver<string> observer)
        {
            return this.subject.Subscribe(observer);
        }

        public void OnNext(ArraySegment<byte> source)
        {
            var offset = source.Offset;
            var end = source.Offset + source.Count;
            for (var i = offset; i < end; ++i)
            {
                if (source.Array[i] == '\r' || (previousByte != '\r' && source.Array[i] == '\n'))
                {
                    this.builder.Append(source.Array, offset, i - offset);
                    this.subject.OnNext(Encoding.UTF8.GetString(this.builder.Array, 0, this.builder.Count));
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

        public void OnError(Exception error)
        {
            this.subject.OnError(error);
        }

        public void OnCompleted()
        {
            if (this.builder.Count > 0)
            {
                this.subject.OnNext(Encoding.UTF8.GetString(this.builder.Array, 0, this.builder.Count));
                this.builder.Clear();
            }
            this.subject.OnCompleted();
        }

        private readonly Subject<string> subject = new Subject<string>();

        private readonly ArrayBuilder<byte> builder = new ArrayBuilder<byte>();

        private int previousByte = -1;
    }
}
