//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using SevenZip;

//namespace KCVDB.LocalAnalyze
//{
//    public sealed class LogSession : IDisposable
//    {
//        public IEnumerable<KCVDBRow> ReadAllRows()
//        {

//        }

//        internal LogSession(string sessionId, List<Tuple<DateTimeOffset, Stream>> logs)
//        {
//            this.logs = logs;
//        }

//        public void Dispose()
//        {
//            this.reader.Dispose();
//        }

//        private readonly List<Tuple<DateTimeOffset, Stream>> logs;
//    }
//}
