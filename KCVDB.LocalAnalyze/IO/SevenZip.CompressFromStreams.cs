//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using SevenZip;

//namespace KCVDB.LocalAnalyze.IO
//{
//    public static partial class SevenZip
//    {
//        public static IEnumerable<KeyValuePair<string, Stream>> CompressFromStreams(FileInfo fileInfo, IDictionary<string, long> namesAndLengths)
//        {
//            var items = new BlockingCollection<KeyValuePair<string, Stream>>();
//            try
//            {
//                Task.Run(() =>
//                {
//                    BlockingIO io = null;
//                    try
//                    {
//                        new SevenZipCompressor().CompressStreamDictionary(
//                            namesAndLengths.ToDictionary(
//                                item => item.Key,
//                                item =>
//                                {
//                                    io?.Dispose();
//                                    io = new BlockingIO(item.Value);
//                                    io.StartingInput += () =>
//                                    {
//                                        items.Add(new KeyValuePair<string, Stream>(item.Key, io.Output));
//                                    };
//                                    return io.Input;
//                                }),
//                            fileInfo.FullName);
//                    }
//                    finally
//                    {
//                        io?.Dispose();
//                        items.CompleteAdding();
//                    }
//                });
//                foreach (var item in items.GetConsumingEnumerable())
//                {
//                    yield return item;
//                }
//            }
//            finally
//            {
//                items.Dispose();
//            }
//        }
//    }
//}
