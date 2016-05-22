//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;

//namespace KCVDB.LocalAnalyze.IO
//{
//    sealed class DirectoryDayEntry : DayEntry
//    {
//        public override IEnumerable<string> SessionIds
//        {
//            get
//            {
//                return this.logPaths.Keys;
//            }
//        }

//        public override void Dispose()
//        {
//        }

//        public override IEnumerable<Log> OpenStreams(IEnumerable<string> sessionIds)
//        {
//            throw new NotImplementedException();
//        }

//        public DirectoryDayEntry(DateTimeOffset date, string dayPath)
//        {
//            this.date = date;
//            this.logPaths = Directory.EnumerateFiles(dayPath).ToDictionary(logPath => Path.GetFileNameWithoutExtension(logPath));
//        }

//        private readonly DateTimeOffset date;
//        private readonly Dictionary<string, string> logPaths;
//    }
//}
