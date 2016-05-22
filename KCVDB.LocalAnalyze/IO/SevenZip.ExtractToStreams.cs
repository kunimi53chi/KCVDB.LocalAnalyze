//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    public static partial class SevenZip
//    {
//        private static IEnumerable<KeyValuePair<string, Stream>> ExtractToStreams(FileInfo fileInfo)
//        {
//            using (var extractor = new SevenZipExtractor(fileInfo.FullName))
//            {
//                foreach (var item in items.GetConsumingEnumerable())
//                {
//                    var task = Task.Run(() =>
//                    {
//                        BlockingIO io = null;
//                        try
//                        {
//                            extractor.ExtractFiles(e =>
//                            {
//                                io?.CompleteAdding();
//                                io = new BlockingIO();
//                                e.ExtractToStream = io.Output;
//                                items.Add(new KeyValuePair<string, Stream>(e.ArchiveFileInfo.FileName, io.Input));
//                            });
//                        }
//                        finally
//                        {
//                            io?.CompleteAdding();
//                            items.CompleteAdding();
//                        }
//                    });
//                    task.Wait();
//                    yield return item;
//                }
//            }
//        }
//    }
//}
////using System;
////using System.Collections.Concurrent;
////using System.Collections.Generic;
////using System.IO;
////using System.Runtime.InteropServices;
////using System.Threading.Tasks;
////using Nomad.Archive.SevenZip;

////namespace KCVDB.LocalAnalyze.IO
////{
////    public static partial class SevenZip
////    {
////        public static IEnumerable<Stream> Extract(Stream archiveStream)
////        {
////            var archive = format.CreateInArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.SevenZip));
////            try
////            {
////                var maxCheckStartPosition = 128UL * 1024UL;
////                if (archive.Open(new InStreamWrapper(archiveStream), ref maxCheckStartPosition, new ArchiveOpenCallback()) != 0)
////                {
////                    throw new IOException();
////                }
////                var numberOfItems = archive.GetNumberOfItems();
////                var indices = new uint[numberOfItems];
////                for (var index = 0U; index < numberOfItems; ++index)
////                {
////                    indices[index] = index;
////                }
////                var streams = new BlockingCollection<Stream>();
////                Task.Run(() =>
////                {
////                    try
////                    {
////                        archive.Extract(indices, numberOfItems, 0, new ArchiveExtractCallback(streams));
////                        streams.CompleteAdding();
////                    }
////                    finally
////                    {
////                        streams.Dispose();
////                    }
////                });
////                return streams;
////            }
////            finally
////            {
////                Marshal.ReleaseComObject(archive);
////            }
////        }

////        private sealed class ArchiveOpenCallback : IArchiveOpenCallback
////        {
////            void IArchiveOpenCallback.SetCompleted(IntPtr files, IntPtr bytes)
////            {
////                throw new NotImplementedException();
////            }

////            void IArchiveOpenCallback.SetTotal(IntPtr files, IntPtr bytes)
////            {
////                throw new NotImplementedException();
////            }
////        }

////        private sealed class ArchiveExtractCallback : IArchiveExtractCallback
////        {
////            public ArchiveExtractCallback(BlockingCollection<Stream> streams)
////            {
////                this.streams = streams;
////            }

////            int IArchiveExtractCallback.GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode)
////            {
////                var io = new BlockingIO();
////                this.streams.Add(io.Input);
////                outStream = new OutStreamWrapper(io.Output);
////                return 0;
////            }

////            void IArchiveExtractCallback.PrepareOperation(AskMode askExtractMode)
////            {
////                throw new NotImplementedException();
////            }

////            void IArchiveExtractCallback.SetCompleted(ref ulong completeValue)
////            {
////                throw new NotImplementedException();
////            }

////            void IArchiveExtractCallback.SetOperationResult(OperationResult resultEOperationResult)
////            {
////                throw new NotImplementedException();
////            }

////            void IArchiveExtractCallback.SetTotal(ulong total)
////            {
////                throw new NotImplementedException();
////            }

////            private readonly BlockingCollection<Stream> streams;
////        }
////    }
////}
