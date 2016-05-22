using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using KCVDB.LocalAnalyze.IO;

namespace KCVDB.LocalAnalyze
{
    public abstract class LogFile
    {
        protected LogFile(Log log, DateTimeOffset date, Guid sessionId)
        {
            this.log = log;
            this.Date = date;
            this.SessionId = sessionId;
            this.readingBuffer = new Subject<ArraySegment<byte>>();
            this.readingBuffer.Subscribe(
                source =>
                {
                    this.readingLines.OnNext(this.ReadLines(source));
                },
                error =>
                {
                    this.readingLines.OnError(error);
                },
                () =>
                {
                    this.readingLines.OnNext(this.ReadLastLines());
                    this.readingLines.OnCompleted();
                });
            this.readingLines = new Subject<IEnumerable<string>>();
            this.ReadingRow = this.readingLines
                .SelectMany(lines => lines
                    .Select(line => new KCVDBRow(line))
                    .Where(row => this.log.StartDateTime <= row.HttpDate && row.HttpDate < this.log.EndDateTime)
                    .ToObservable())
                .AsObservable();
        }

        private IEnumerable<string> ReadLines(ArraySegment<byte> source)
        {
            var offset = source.Offset;
            var end = source.Offset + source.Count;
            for (var i = offset; i < end; ++i)
            {
                if (source.Array[i] == '\r' || (source.Array[i] == '\n' && !this.isPreviousCr))
                {
                    this.builder.Append(source.Array, offset, i - offset);
                    yield return Encoding.UTF8.GetString(this.builder.Array, 0, this.builder.Count);
                    this.builder.Clear();
                }
                if (source.Array[i] == '\r' || source.Array[i] == '\n')
                {
                    offset = i + 1;
                }
                this.isPreviousCr = source.Array[i] == '\r';
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

        private readonly Log log;

        public readonly DateTimeOffset Date;

        public readonly Guid SessionId;

        protected readonly Subject<ArraySegment<byte>> readingBuffer;

        private readonly Subject<IEnumerable<string>> readingLines;

        public readonly IObservable<KCVDBRow> ReadingRow;

        private readonly ArrayBuilder<byte> builder = new ArrayBuilder<byte>();

        private bool isPreviousCr;
    }
}
