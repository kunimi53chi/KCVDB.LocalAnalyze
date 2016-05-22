//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    public sealed class ArchiveDayEntry : DayEntry
//    {
//        public override IEnumerable<string> SessionIds
//        {
//            get
//            {
//                return logPaths.Keys;
//            }
//        }

//        public override void Dispose()
//        {
//            this.extractor.Dispose();
//        }

//        public override IEnumerable<Log> OpenStreams(IEnumerable<string> sessionIds)
//        {
//        }

//        public ArchiveDayEntry(DateTimeOffset date, string dayPath)
//        {
//            this.date = date;
//            this.extractor = new SevenZipExtractor(dayPath);
//            this.logPaths = this.extractor.ArchiveFileNames.ToDictionary(logPath => Path.GetFileNameWithoutExtension(logPath));
//        }

//        private readonly DateTimeOffset date;
//        private readonly SevenZipExtractor extractor;
//        private readonly Dictionary<string, string> logPaths;
//    }
//}
