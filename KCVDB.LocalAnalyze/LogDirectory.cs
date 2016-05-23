using System;
using System.IO;
using System.IO.Compression;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using KCVDB.LocalAnalyze.IO;

namespace KCVDB.LocalAnalyze
{
    public sealed class LogDirectory : IObservable<LogFile>
    {
        public LogDirectory(string path)
        {
            this.Path = path;
        }

        public IDisposable Subscribe(IObserver<LogFile> observer)
        {
            var subject = new Subject<LogFile>();
            var result = subject.Subscribe(observer);
            var directoryInfo = new DirectoryInfo(this.Path);
            if (directoryInfo.Exists)
            {
                foreach (var fileInfo in directoryInfo.GetFiles("*", SearchOption.AllDirectories))
                {
                    this.Read(subject, fileInfo);
                }
            }
            else
            {
                var fileInfo = new FileInfo(this.Path);
                if (fileInfo.Exists)
                {
                    this.Read(subject, fileInfo);
                }
            }
            subject.OnCompleted();
            subject.Dispose();
            return result;
        }

        private void Read(Subject<LogFile> subject, FileInfo fileInfo)
        {
            if (fileInfo.Extension == ".log" || fileInfo.Extension == ".gz")
            {
                Stream stream = fileInfo.OpenRead();
                if (fileInfo.Extension == ".gz")
                {
                    stream = new GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                }
                using (stream)
                {
                    subject.OnNext(new StreamLogFile(this, fileInfo.FullName, stream));
                }
            }
            else if (fileInfo.Extension == ".7z")
            {
                new SevenZipExtractor(fileInfo.FullName).Subscribe(archiveFile =>
                {
                    subject.OnNext(new ArchiveLogFile(this, fileInfo.FullName, archiveFile));
                });
            }
            else
            {
                throw new NotSupportedException();
            }
        }

        public readonly string Path;
    }
}
