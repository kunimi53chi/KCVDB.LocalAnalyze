using System;
using System.Reactive.Linq;
using Microsoft.COM;
using Nomad.Archive.SevenZip;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class SevenZipArchiveFile : IObservable<ArraySegment<byte>>
    {
        public IDisposable Subscribe(IObserver<ArraySegment<byte>> observer)
        {
            return this.observable.Subscribe(observer);
        }

        public SevenZipArchiveFile(IInArchive archive, uint index, IObservable<ArraySegment<byte>> observable)
        {
            this.observable = observable.Publish().RefCount();
            this.archive = archive;
            this.index = index;
        }

        public string Path
        {
            get
            {
                if (this.path == null)
                {
                    var value = new PropVariant();
                    this.archive.GetProperty(this.index, ItemPropId.kpidPath, ref value);
                    this.path = (string)value.GetObject();
                }
                return this.path;
            }
        }

        public bool Cancel;

        private readonly uint index;

        private readonly IInArchive archive;

        private string path;

        private readonly IObservable<ArraySegment<byte>> observable;
    }
}
