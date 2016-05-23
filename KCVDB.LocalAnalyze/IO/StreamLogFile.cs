using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace KCVDB.LocalAnalyze.IO
{
    sealed class StreamLogFile : LogFile
    {
        public StreamLogFile(Log manager, string path, Stream stream)
            : base(manager)
        {
            this.path = path;
            this.stream = stream;
        }

        public override string Path
        {
            get
            {
                return this.path;
            }
        }

        public override IDisposable Subscribe(IObserver<string> observer)
        {
            var subject = new Subject<ArraySegment<byte>>();
            var disposable = subject.ReadLine().Subscribe(observer);
            var buffer = new byte[1 << 21];
            int count;
            while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                subject.OnNext(new ArraySegment<byte>(buffer, 0, count));
            }
            subject.OnCompleted();
            return disposable;
        }

        private readonly string path;

        private readonly Stream stream;
    }
}
