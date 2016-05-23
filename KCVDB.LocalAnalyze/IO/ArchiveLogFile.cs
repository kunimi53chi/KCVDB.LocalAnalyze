using System;
using System.Reactive.Linq;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class ArchiveLogFile : LogFile
    {
        public ArchiveLogFile(Log log, string path, SevenZipArchiveFile archiveFile)
            : base(log)
        {
            this.archiveFile = archiveFile;
            this.filePath = path;
            this.archiveFile.Cancel = true;
        }

        public override string Path
        {
            get
            {
                if (this.path == null)
                {
                    this.path = System.IO.Path.Combine(this.filePath, this.archiveFile.Path);
                }
                return this.path;
            }
        }

        private readonly string filePath;

        private string path;

        public override IDisposable Subscribe(IObserver<string> observer)
        {
            if (this.observable == null)
            {
                this.observable = archiveFile.ReadLine().Publish().RefCount();
            }
            var disposable = this.observable.Subscribe(observer);
            this.archiveFile.Cancel = false;
            return disposable;
        }

        private readonly SevenZipArchiveFile archiveFile;

        private IObservable<string> observable;
    }
}
