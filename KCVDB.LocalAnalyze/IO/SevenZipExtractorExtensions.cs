//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading.Tasks;
//using SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    static class SevenZipExtractorExtensions
//    {
//        public static Stream Open(this SevenZipExtractor extractor, int index)
//        {
//            return Open(stream => extractor.ExtractFile(index, stream));
//        }

//        public static Stream Open(this SevenZipExtractor extractor, string fileName)
//        {
//            return Open(stream => extractor.ExtractFile(fileName, stream));
//        }

//        public static Stream Open(this SevenZipExtractor extractor, ArchiveFileInfo archiveFileInfo)
//        {
//            return Open(stream => extractor.ExtractFile(archiveFileInfo.Index, stream));
//        }

//        private static Stream Open(this SevenZipExtractor extractor, IEnumerable<string> fileNames)
//        {
//            var streams = new BlockingIO();
//            Task.Run(() =>
//            {
//                try
//                {

//                }
//                finally
//                {
//                    streams.Output.Dispose();
//                }
//            });
//            return streams.Input;
//        }
//    }
//}
