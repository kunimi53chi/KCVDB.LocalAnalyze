//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.InteropServices;
//using System.Text;
//using System.Threading.Tasks;
//using Nomad.Archive.SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    public static partial class SevenZip
//    {
//        public static void Compress(IEnumerable<KeyValuePair<string, Stream>> streams, Stream archiveStream)
//        {
//            var archive = format.CreateOutArchive(SevenZipFormat.GetClassIdFromKnownFormat(KnownSevenZipFormat.SevenZip));
//            try
//            {
//                archive.UpdateItems(new OutStreamWrapper(archiveStream), , new ArchiveUpdateCallback());
//            }
//            finally
//            {
//                Marshal.ReleaseComObject(archive);
//            }
//        }

//        private sealed class ArchiveUpdateCallback : IArchiveUpdateCallback2
//        {
//            void IArchiveUpdateCallback2.GetProperty(int index, ItemPropId propID, IntPtr value)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.GetStream(int index, out ISequentialInStream inStream)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.GetUpdateItemInfo(int index, out int newData, out int newProperties, out uint indexInArchive)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.GetVolumeSize(int index, out ulong size)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.GetVolumeStream(int index, out ISequentialOutStream volumeStream)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.SetCompleted(ref ulong completeValue)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.SetOperationResult(int operationResult)
//            {
//                throw new NotImplementedException();
//            }

//            void IArchiveUpdateCallback2.SetTotal(ulong total)
//            {
//                throw new NotImplementedException();
//            }
//        }
//    }
//}
