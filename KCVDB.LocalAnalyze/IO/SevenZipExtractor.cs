using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.COM;
using Nomad.Archive.SevenZip;

namespace KCVDB.LocalAnalyze.IO
{
    internal sealed class SevenZipExtractor : IObservable<SevenZipArchiveFile>
    {
        public SevenZipExtractor(string name)
        {
            this.Name = name;
        }

        public IDisposable Subscribe(IObserver<SevenZipArchiveFile> observer)
        {
            var subject = new Subject<SevenZipArchiveFile>();
            subject.Subscribe(observer);
            var archive = format.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.SevenZip));
            try
            {
                using (var fileStream = File.OpenRead(this.Name))
                {
                    var maxCheckStartPosition = 128UL * 1024UL;
                    if (archive.Open(new InStreamWrapper(fileStream), ref maxCheckStartPosition, new ArchiveOpenCallback()) != 0)
                    {
                        throw new IOException();
                    }
                    var indices = Enumerable.Range(0, (int)archive.GetNumberOfItems())
                        .Select(index => (uint)index)
                        .Where(index =>
                        {
                            var value = new PropVariant();
                            archive.GetProperty(index, ItemPropId.kpidIsFolder, ref value);
                            return !(bool)value.GetObject();
                        })
                        .ToArray();
                    var callback = new ArchiveExtractCallback(archive);
                    callback.Subscribe(subject);
                    archive.Extract(indices, (uint)indices.Length, 0, callback);
                }
                subject.OnCompleted();
                return subject;
            }
            finally
            {
                Marshal.ReleaseComObject(archive);
            }
        }

        private readonly string Name;

        private sealed class ArchiveOpenCallback : IArchiveOpenCallback
        {
            void IArchiveOpenCallback.SetCompleted(IntPtr files, IntPtr bytes)
            {
                throw new NotImplementedException();
            }

            void IArchiveOpenCallback.SetTotal(IntPtr files, IntPtr bytes)
            {
                throw new NotImplementedException();
            }
        }

        private sealed class ArchiveExtractCallback : IArchiveExtractCallback, IObservable<SevenZipArchiveFile>
        {
            int IArchiveExtractCallback.GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode)
            {
                var stream = new ObservableStream();
                var archiveFile = new SevenZipArchiveFile(this.archive, index, stream);
                this.subject.OnNext(archiveFile);
                if (archiveFile.Cancel)
                {
                    outStream = null;
                }
                else
                {
                    outStream = new OutStreamWrapper(stream);
                }
                return 0;
            }

            void IArchiveExtractCallback.PrepareOperation(AskMode askExtractMode)
            {
            }

            void IArchiveExtractCallback.SetCompleted(ref ulong completeValue)
            {
            }

            void IArchiveExtractCallback.SetOperationResult(OperationResult resultEOperationResult)
            {
            }

            void IArchiveExtractCallback.SetTotal(ulong total)
            {
            }

            public IDisposable Subscribe(IObserver<SevenZipArchiveFile> observer)
            {
                return this.subject.Subscribe(observer);
            }

            public ArchiveExtractCallback(IInArchive archive)
            {
                this.archive = archive;
            }

            private readonly Subject<SevenZipArchiveFile> subject = new Subject<SevenZipArchiveFile>();

            private readonly IInArchive archive;
        }

        static SevenZipExtractor()
        {
            if (IntPtr.Size == 4)
            {
                format = new SevenZipFormat(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x86", "7z.dll"));
            }
            else if (IntPtr.Size == 8)
            {
                format = new SevenZipFormat(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x64", "7z.dll"));
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }

        private static readonly SevenZipFormat format;
    }
}
