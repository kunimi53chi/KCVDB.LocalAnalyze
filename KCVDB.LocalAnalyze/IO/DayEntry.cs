//using System;
//using System.Collections.Generic;
//using System.IO;

//namespace KCVDB.LocalAnalyze.IO
//{
//    internal abstract class DayEntry : IDisposable
//    {
//        public static DayEntry Create(DateTimeOffset date, string dayPath)
//        {
//            if (File.Exists(dayPath))
//            {
//                return new ArchiveDayEntry(date, dayPath);
//            }
//            else if (Directory.Exists(dayPath))
//            {
//                return new DirectoryDayEntry(date, dayPath);
//            }
//            else
//            {
//                throw new FileNotFoundException();
//            }
//        }

//        public abstract IEnumerable<string> SessionIds { get; }
//        public abstract void Dispose();
//        public abstract Stream OpenStream(string sessionId);
//        public abstract IEnumerable<Tuple<string, Stream>> OpenStreams(IEnumerable<string> sessionIds);
//    }
//}
