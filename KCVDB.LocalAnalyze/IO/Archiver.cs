//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Threading.Tasks;
//using SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    public static class Archiver
//    {
//        public static IEnumerable<KeyValuePair<string, Stream>> ExtractToStreams(FileInfo fileInfo)
//        {
//            using (var extractor = new SevenZipExtractor(fileInfo.FullName))
//            {
//                extractor.ExtractFiles(e =>
//                {
//                    e.ExtractToStream = 
//                });
//            }
//        }

//        static Archiver()
//        {
//            if (IntPtr.Size == 4)
//            {
//                SevenZipBase.SetLibraryPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x86", "7z.dll"));
//            }
//            else if (IntPtr.Size == 8)
//            {
//                SevenZipBase.SetLibraryPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x64", "7z.dll"));
//            }
//            else
//            {
//                throw new PlatformNotSupportedException();
//            }
//        }
//    }
//}
////using System;
////using System.IO;
////using System.Reflection;
////using Nomad.Archive.SevenZip;

////namespace KCVDB.LocalAnalyze.IO
////{
////    public static partial class SevenZip
////    {
////        static SevenZip()
////        {
////            if (IntPtr.Size == 4)
////            {
////                format = new SevenZipFormat(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x86", "7z.dll"));
////            }
////            else if (IntPtr.Size == 8)
////            {
////                format = new SevenZipFormat(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "x64", "7z.dll"));
////            }
////            else
////            {
////                throw new PlatformNotSupportedException();
////            }
////        }

////        private static readonly SevenZipFormat format;
////    }
////}
